;
//asignar un nombre y versión al cache
const CACHE_NAME = 'v1_pwa_app_cache',
    urlsToCache = [
        './',
        '/home/index', // Ruta al Home/Index
        '/img_plantilla/apple-icon.png',  // Icono para dispositivos Apple
        '/css_plantilla/bootstrap.min.css',  // Bootstrap CSS
        '/css_plantilla/templatemo.css',  // Archivo de estilos templatemo
        '/css_plantilla/custom.css',  // Estilos personalizados
        '/css_plantilla/fontawesome.min.css',  // FontAwesome para iconos
        'https://fonts.googleapis.com/css2?family=Roboto:wght@100;200;300;400;500;700;900&display=swap'  // Fuentes externas

    ]

//durante la fase de instalación, generalmente se almacena en caché los activos estáticos
self.addEventListener('install', e => {
    e.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                return cache.addAll(urlsToCache)
                    .then(() => self.skipWaiting())
            })
            .catch(err => console.log('Falló registro de cache', err))
    )
})

//una vez que se instala el SW, se activa y busca los recursos para hacer que funcione sin conexión
self.addEventListener('activate', e => {
    const cacheWhitelist = [CACHE_NAME]

    e.waitUntil(
        caches.keys()
            .then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => {
                        //Eliminamos lo que ya no se necesita en cache
                        if (cacheWhitelist.indexOf(cacheName) === -1) {
                            return caches.delete(cacheName)
                        }
                    })
                )
            })
            // Le indica al SW activar el cache actual
            .then(() => self.clients.claim())
    )
})

//cuando el navegador recupera una url
self.addEventListener('fetch', e => {
    //Responder ya sea con el objeto en caché o continuar y buscar la url real
    e.respondWith(
        caches.match(e.request)
            .then(res => {
                if (res) {
                    //recuperar del cache
                    return res
                }
                //recuperar de la petición a la url
                return fetch(e.request)
            }).catch(err => console.log('Falló algo al solicitar recursos', err))
    )
})
