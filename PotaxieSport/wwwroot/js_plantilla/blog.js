document.addEventListener("DOMContentLoaded", function () {

    function fetchLatestNews() {
        fetch('https://newsapi.org/v2/top-headlines?country=us&category=sports&apiKey=a43f9dff52e44b43ba04df441b44b3c4')
            .then(response => response.json())
            .then(data => {
                const newsList = document.getElementById('news-list');
                data.articles.forEach(article => {
                    const articleElement = document.createElement('div');
                    articleElement.classList.add('col-lg-4', 'mb-5');
                    articleElement.innerHTML = `
                                <div class="card rounded-3 shadow-sm border-0">
                                    <img src="${article.urlToImage}" class="card-img-top rounded-3" alt="${article.title}">
                                    <div class="card-body p-4">
                                        <h5 class="card-title"><a href="${article.url}" target="_blank">${article.title}</a></h5>
                                        <p class="card-text">${article.description}</p>
                                    </div>
                                </div>
                            `;
                    newsList.appendChild(articleElement);
                });
            })
            .catch(error => console.error('Error fetching latest news:', error));
    }




    fetchLatestNews();

});