PGDMP  "                    |            potaxie_sport1_t2tl    16.3 (Debian 16.3-1.pgdg120+1)    16.3 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16389    potaxie_sport1_t2tl    DATABASE     ~   CREATE DATABASE potaxie_sport1_t2tl WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.UTF8';
 #   DROP DATABASE potaxie_sport1_t2tl;
                potaxie_sport1    false            �           0    0    potaxie_sport1_t2tl    DATABASE PROPERTIES     <   ALTER DATABASE potaxie_sport1_t2tl SET "TimeZone" TO 'utc';
                     potaxie_sport1    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                potaxie_sport1    false                       1255    16626 �   actualizar_usuario(integer, character varying, character varying, character varying, character varying, character varying, integer)    FUNCTION       CREATE FUNCTION public.actualizar_usuario(p_usuario_id integer, p_nombre character varying, p_ap_paterno character varying, p_ap_materno character varying, p_username character varying, p_email character varying, p_rol_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public.usuario
    SET 
        nombre = p_nombre,
        ap_paterno = p_ap_paterno,
        ap_materno = p_ap_materno,
        username = p_username,
        email = p_email,
        rol_id = p_rol_id
    WHERE usuario_id = p_usuario_id;
END;
$$;
 �   DROP FUNCTION public.actualizar_usuario(p_usuario_id integer, p_nombre character varying, p_ap_paterno character varying, p_ap_materno character varying, p_username character varying, p_email character varying, p_rol_id integer);
       public          potaxie_sport1    false    5            �            1255    16399    agregarintentofallido(integer)    FUNCTION     �   CREATE FUNCTION public.agregarintentofallido(id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE usuario SET error_autentificacion = error_autentificacion+1 WHERE usuario_id = id;
END;
$$;
 8   DROP FUNCTION public.agregarintentofallido(id integer);
       public          potaxie_sport1    false    5            �            1255    16400 .   cambiarcontraseña(integer, character varying)    FUNCTION     �   CREATE FUNCTION public."cambiarcontraseña"(id integer, lapoderosa character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE usuario SET password = lapoderosa WHERE usuario_id = id;
END;
$$;
 U   DROP FUNCTION public."cambiarcontraseña"(id integer, lapoderosa character varying);
       public          potaxie_sport1    false    5            �            1255    16401 $   crearregistrosalud(integer, integer)    FUNCTION     �  CREATE FUNCTION public.crearregistrosalud(p_jugador_id integer, p_frecuencia_card integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_estatus VARCHAR;
BEGIN
    -- Determinar el estatus basado en la frecuencia cardíaca
    IF p_frecuencia_card BETWEEN 50 AND 60 THEN
        v_estatus := 'bajo';
    ELSIF p_frecuencia_card BETWEEN 100 AND 110 THEN
        v_estatus := 'alto';
    ELSE
        v_estatus := 'normal'; -- Si la frecuencia cardíaca no está en los rangos específicos
    END IF;

    -- Insertar el registro en la tabla registro_salud
    INSERT INTO registro_salud (jugador_id, frecuencia_card, estatus, fecha)
    VALUES (p_jugador_id, p_frecuencia_card, v_estatus, CURRENT_DATE);
END;
$$;
 Z   DROP FUNCTION public.crearregistrosalud(p_jugador_id integer, p_frecuencia_card integer);
       public          potaxie_sport1    false    5            �            1255    16402 B   crearusuario(text, text, text, text, text, integer, integer, text)    FUNCTION     i  CREATE FUNCTION public.crearusuario(p_nombre text, p_ap_paterno text, p_ap_materno text, p_username text, p_email text, p_rol_id integer, p_error_autentificacion integer, p_password text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO usuario (
        nombre,
        ap_paterno,
        ap_materno,
        username,
        email,
        rol_id,
        error_autentificacion,
        password
    ) VALUES (
        p_nombre,
        p_ap_paterno,
        p_ap_materno,
        p_username,
        p_email,
        p_rol_id,
        p_error_autentificacion,
        p_password
    );
END;
$$;
 �   DROP FUNCTION public.crearusuario(p_nombre text, p_ap_paterno text, p_ap_materno text, p_username text, p_email text, p_rol_id integer, p_error_autentificacion integer, p_password text);
       public          potaxie_sport1    false    5            �            1255    16403    limpiarintentofallido(integer)    FUNCTION     �   CREATE FUNCTION public.limpiarintentofallido(id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE usuario SET error_autentificacion = 0 WHERE usuario_id = id;
END;
$$;
 8   DROP FUNCTION public.limpiarintentofallido(id integer);
       public          potaxie_sport1    false    5                       1255    16625    obtener_roles()    FUNCTION     �   CREATE FUNCTION public.obtener_roles() RETURNS TABLE(rol_id integer, rol_nombre character varying)
    LANGUAGE sql
    AS $$
    SELECT rol_id, rol_nombre
    FROM public.rol;
$$;
 &   DROP FUNCTION public.obtener_roles();
       public          potaxie_sport1    false    5            �            1255    16404    obtenerequipos()    FUNCTION     �  CREATE FUNCTION public.obtenerequipos() RETURNS TABLE(equipo_id integer, nombre_equipo character varying, genero character varying, logo character varying, categoria_id integer, categoria_nombre character varying, usuario_coach integer, nombre_coach character varying, torneo_actual integer, torneo character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        e.equipo_id,
        e.nombre_equipo,
        e.genero,
        e.logo,
        e.categoria_id,
        c.categoria_nombre,
        e.usuario_coach,
        (SELECT CONCAT(u.nombre, ' ', u.ap_paterno, ' ', u.ap_materno)::VARCHAR 
         FROM usuario u WHERE u.usuario_id = e.usuario_coach) AS coach,
		e.torneo_actual,
		(SELECT t.nombre_torneo::VARCHAR
         FROM torneo t WHERE t.torneo_id = e.torneo_actual) AS torneo
    FROM equipo e
    LEFT JOIN categoria c ON e.categoria_id = c.categoria_id;
END;
$$;
 '   DROP FUNCTION public.obtenerequipos();
       public          potaxie_sport1    false    5                        1255    16405 "   obtenerjugadoresporequipo(integer)    FUNCTION     �  CREATE FUNCTION public.obtenerjugadoresporequipo(p_equipo_id integer) RETURNS TABLE(jugador_id integer, jugador_nombre character varying, ap_paterno character varying, ap_materno character varying, edad integer, fotografia character varying, equipo_id integer, equipo character varying, posicion character varying, num_jugador integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        j.jugador_id,
        j.nombre AS jugador_nombre,
        j.ap_paterno,
        j.ap_materno,
        j.edad,
        j.fotografia,
        j.equipo_id,
        e.nombre_equipo AS equipo,
        j.posicion,
        j.num_jugador
    FROM jugador j
    LEFT JOIN equipo e ON j.equipo_id = e.equipo_id
    WHERE j.equipo_id = p_equipo_id;
END;
$$;
 E   DROP FUNCTION public.obtenerjugadoresporequipo(p_equipo_id integer);
       public          potaxie_sport1    false    5                       1255    16406    obtenerpartidos()    FUNCTION       CREATE FUNCTION public.obtenerpartidos() RETURNS TABLE(partido_id integer, torneo_id integer, torneo character varying, equipo_retador integer, retador character varying, equipo_defensor integer, defensor character varying, equipo_ganador integer, ganador character varying, usuario_arbitro integer, arbitro character varying, cedula character varying, fecha date, hora time without time zone, lugar character varying, costo money)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.partido_id,
		p.torneo_id,
		(SELECT t.nombre_torneo::VARCHAR
         FROM torneo t WHERE t.torneo_id = p.torneo_id) AS torneo,
        p.equipo_retador,
		(SELECT e.nombre_equipo::VARCHAR 
         FROM equipo e WHERE e.equipo_id = p.equipo_retador) AS retador,
		p.equipo_defensor,
		(SELECT e.nombre_equipo::VARCHAR
         FROM equipo e WHERE e.equipo_id = p.equipo_defensor) AS defensor,
		p.equipo_ganador,
		(SELECT e.nombre_equipo::VARCHAR 
         FROM equipo e WHERE e.equipo_id = p.equipo_ganador) AS ganador,
		p.usuario_arbitro,
		(SELECT CONCAT(u.nombre, ' ', u.ap_paterno, ' ', u.ap_materno)::VARCHAR 
         FROM usuario u WHERE u.usuario_id = p.usuario_arbitro) AS arbitro,
        p.cedula,
        p.fecha,
        p.hora,
        p.lugar,
		p.costo
    FROM partido p;
END;
$$;
 (   DROP FUNCTION public.obtenerpartidos();
       public          potaxie_sport1    false    5                       1255    16407    obtenerregistrossalud(integer)    FUNCTION     �  CREATE FUNCTION public.obtenerregistrossalud(p_jugador_id integer) RETURNS TABLE(registro_salud_id integer, jugador_id integer, jugador character varying, frecuencia_card integer, estatus character varying, fecha date)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        rs.registro_salud_id,
        rs.jugador_id,
        CONCAT(j.nombre, ' ', j.ap_paterno, ' ', j.ap_materno)::varchar AS jugador,
        rs.frecuencia_card,
        rs.estatus,
        rs.fecha
    FROM 
        registro_salud rs
    JOIN 
        jugador j ON rs.jugador_id = j.jugador_id
    WHERE 
        rs.jugador_id = p_jugador_id;
END;
$$;
 B   DROP FUNCTION public.obtenerregistrossalud(p_jugador_id integer);
       public          potaxie_sport1    false    5                       1255    16408    obtenertorneos()    FUNCTION     �  CREATE FUNCTION public.obtenertorneos() RETURNS TABLE(torneo_id integer, nombre_torneo character varying, categoria_id integer, categoria character varying, genero character varying, logo character varying, usuario_admin integer, administrador character varying, usuario_contador integer, contador character varying, usuario_doctor integer, doctor character varying, fecha_inicio date, fecha_fin date)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        t.torneo_id,
        t.nombre_torneo,
        t.categoria_id,
		(SELECT c.categoria_nombre FROM categoria c WHERE c.categoria_id = t.categoria_id) AS categoria,
        t.genero,
        t.logo,
        t.usuario_admin,
		(SELECT CONCAT(u.nombre, ' ', u.ap_paterno, ' ', u.ap_materno) ::VARCHAR  FROM usuario u WHERE u.usuario_id = t.usuario_admin) AS administrador,
        t.usuario_contador,
		(SELECT CONCAT(u.nombre, ' ', u.ap_paterno, ' ', u.ap_materno) ::VARCHAR  FROM usuario u WHERE u.usuario_id = t.usuario_contador) AS contador,
        t.usuario_doctor,
		(SELECT CONCAT(u.nombre, ' ', u.ap_paterno, ' ', u.ap_materno) ::VARCHAR  FROM usuario u WHERE u.usuario_id = t.usuario_doctor) AS doctor,
        t.fecha_inicio,
        t.fecha_fin
    FROM torneo t;
END;
$$;
 '   DROP FUNCTION public.obtenertorneos();
       public          potaxie_sport1    false    5                       1255    16628    obtenerusuarioporid(integer)    FUNCTION       CREATE FUNCTION public.obtenerusuarioporid(p_id integer) RETURNS TABLE(usuario_id integer, nombre character varying, ap_paterno character varying, ap_materno character varying, username character varying, email character varying, rol_id integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT
        u.usuario_id,
        u.nombre,
        u.ap_paterno,
        u.ap_materno,
        u.username,
        u.email,
        u.rol_id
    FROM
        public.usuario u
    WHERE
        u.usuario_id = p_id;
END;
$$;
 8   DROP FUNCTION public.obtenerusuarioporid(p_id integer);
       public          potaxie_sport1    false    5                       1255    16409 ,   obtenerusuarioporusername(character varying)    FUNCTION     �  CREATE FUNCTION public.obtenerusuarioporusername(p_username character varying) RETURNS TABLE(usuario_id integer, nombre character varying, ap_paterno character varying, ap_materno character varying, username character varying, email character varying, password bytea, rol_id integer, rol character varying, error_autentificacion integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        u.usuario_id,
        u.nombre,
        u.ap_paterno,
        u.ap_materno,
        u.username,
        u.email,
        u.password,
        u.rol_id,
        r.rol_nombre AS rol,
        u.error_autentificacion
    FROM 
        usuario u
    JOIN 
        rol r ON u.rol_id = r.rol_id
    WHERE 
        u.username = p_username;
END;
$$;
 N   DROP FUNCTION public.obtenerusuarioporusername(p_username character varying);
       public          potaxie_sport1    false    5                       1255    16410 !   validarusuario(character varying)    FUNCTION     �  CREATE FUNCTION public.validarusuario(p_correo character varying) RETURNS TABLE(usuario_id integer, nombre character varying, ap_paterno character varying, ap_materno character varying, username character varying, email character varying, password character varying, rol_id integer, rol character varying, error_autentificacion integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT 
        u.usuario_id,
        u.nombre,
        u.ap_paterno,
        u.ap_materno,
        u.username,
        u.email,
        u.password,
        u.rol_id,
        r.rol_nombre AS rol,
        u.error_autentificacion
    FROM 
        usuario u
    JOIN 
        rol r ON u.rol_id = r.rol_id
    WHERE 
        u.email = p_correo;
END;
$$;
 A   DROP FUNCTION public.validarusuario(p_correo character varying);
       public          potaxie_sport1    false    5            �            1259    16411 	   categoria    TABLE     �   CREATE TABLE public.categoria (
    categoria_id integer NOT NULL,
    categoria_nombre character varying(50) NOT NULL,
    rango character varying(50) NOT NULL
);
    DROP TABLE public.categoria;
       public         heap    potaxie_sport1    false    5            �            1259    16414    categoria_categoria_id_seq    SEQUENCE     �   CREATE SEQUENCE public.categoria_categoria_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public.categoria_categoria_id_seq;
       public          potaxie_sport1    false    215    5            �           0    0    categoria_categoria_id_seq    SEQUENCE OWNED BY     Y   ALTER SEQUENCE public.categoria_categoria_id_seq OWNED BY public.categoria.categoria_id;
          public          potaxie_sport1    false    216            �            1259    16415    disponibilidad_arbitro    TABLE     �   CREATE TABLE public.disponibilidad_arbitro (
    disp_arb_id integer NOT NULL,
    usuario_id integer NOT NULL,
    dia character varying(50) NOT NULL,
    hora_inicio time without time zone NOT NULL,
    hora_final time without time zone NOT NULL
);
 *   DROP TABLE public.disponibilidad_arbitro;
       public         heap    potaxie_sport1    false    5            �            1259    16418 &   disponibilidad_arbitro_disp_arb_id_seq    SEQUENCE     �   CREATE SEQUENCE public.disponibilidad_arbitro_disp_arb_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 =   DROP SEQUENCE public.disponibilidad_arbitro_disp_arb_id_seq;
       public          potaxie_sport1    false    217    5            �           0    0 &   disponibilidad_arbitro_disp_arb_id_seq    SEQUENCE OWNED BY     q   ALTER SEQUENCE public.disponibilidad_arbitro_disp_arb_id_seq OWNED BY public.disponibilidad_arbitro.disp_arb_id;
          public          potaxie_sport1    false    218            �            1259    16419    equipo    TABLE     (  CREATE TABLE public.equipo (
    equipo_id integer NOT NULL,
    nombre_equipo character varying(50) NOT NULL,
    genero character varying(50) NOT NULL,
    logo character varying(100) NOT NULL,
    categoria_id integer NOT NULL,
    usuario_coach integer NOT NULL,
    torneo_actual integer
);
    DROP TABLE public.equipo;
       public         heap    potaxie_sport1    false    5            �            1259    16422    equipo_equipo_id_seq    SEQUENCE     �   CREATE SEQUENCE public.equipo_equipo_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.equipo_equipo_id_seq;
       public          potaxie_sport1    false    219    5            �           0    0    equipo_equipo_id_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public.equipo_equipo_id_seq OWNED BY public.equipo.equipo_id;
          public          potaxie_sport1    false    220            �            1259    16423    jugador    TABLE     �  CREATE TABLE public.jugador (
    jugador_id integer NOT NULL,
    nombre character varying(50) NOT NULL,
    ap_paterno character varying(50) NOT NULL,
    ap_materno character varying(50) NOT NULL,
    edad integer NOT NULL,
    fotografia character varying(100) NOT NULL,
    equipo_id integer NOT NULL,
    posicion character varying(50) NOT NULL,
    num_jugador integer NOT NULL
);
    DROP TABLE public.jugador;
       public         heap    potaxie_sport1    false    5            �            1259    16426    jugador_jugador_id_seq    SEQUENCE     �   CREATE SEQUENCE public.jugador_jugador_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.jugador_jugador_id_seq;
       public          potaxie_sport1    false    5    221            �           0    0    jugador_jugador_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.jugador_jugador_id_seq OWNED BY public.jugador.jugador_id;
          public          potaxie_sport1    false    222            �            1259    16427    movimiento_economico    TABLE       CREATE TABLE public.movimiento_economico (
    movimiento_id integer NOT NULL,
    fecha date NOT NULL,
    contador_id integer NOT NULL,
    tipo character varying(50) NOT NULL,
    cantidad money NOT NULL,
    torneo_id integer NOT NULL,
    comprobante character varying(100)
);
 (   DROP TABLE public.movimiento_economico;
       public         heap    potaxie_sport1    false    5            �            1259    16430 &   movimiento_economico_movimiento_id_seq    SEQUENCE     �   CREATE SEQUENCE public.movimiento_economico_movimiento_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 =   DROP SEQUENCE public.movimiento_economico_movimiento_id_seq;
       public          potaxie_sport1    false    223    5            �           0    0 &   movimiento_economico_movimiento_id_seq    SEQUENCE OWNED BY     q   ALTER SEQUENCE public.movimiento_economico_movimiento_id_seq OWNED BY public.movimiento_economico.movimiento_id;
          public          potaxie_sport1    false    224            �            1259    16431    pago_partido    TABLE     �   CREATE TABLE public.pago_partido (
    pago_partido_id integer NOT NULL,
    equipo_id integer NOT NULL,
    partido_id integer NOT NULL,
    completado bit(1) NOT NULL,
    fecha_pago date,
    comprobante character varying(100)
);
     DROP TABLE public.pago_partido;
       public         heap    potaxie_sport1    false    5            �            1259    16434     pago_partido_pago_partido_id_seq    SEQUENCE     �   CREATE SEQUENCE public.pago_partido_pago_partido_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 7   DROP SEQUENCE public.pago_partido_pago_partido_id_seq;
       public          potaxie_sport1    false    225    5            �           0    0     pago_partido_pago_partido_id_seq    SEQUENCE OWNED BY     e   ALTER SEQUENCE public.pago_partido_pago_partido_id_seq OWNED BY public.pago_partido.pago_partido_id;
          public          potaxie_sport1    false    226            �            1259    16435    participaciones    TABLE     �   CREATE TABLE public.participaciones (
    participacion_id integer NOT NULL,
    equipo_id integer NOT NULL,
    torneo_id integer NOT NULL,
    activo bit(1) NOT NULL
);
 #   DROP TABLE public.participaciones;
       public         heap    potaxie_sport1    false    5            �            1259    16438 $   participaciones_participacion_id_seq    SEQUENCE     �   CREATE SEQUENCE public.participaciones_participacion_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ;   DROP SEQUENCE public.participaciones_participacion_id_seq;
       public          potaxie_sport1    false    227    5            �           0    0 $   participaciones_participacion_id_seq    SEQUENCE OWNED BY     m   ALTER SEQUENCE public.participaciones_participacion_id_seq OWNED BY public.participaciones.participacion_id;
          public          potaxie_sport1    false    228            �            1259    16439    partido    TABLE     ~  CREATE TABLE public.partido (
    partido_id integer NOT NULL,
    torneo_id integer NOT NULL,
    equipo_retador integer NOT NULL,
    equipo_defensor integer NOT NULL,
    equipo_ganador integer,
    usuario_arbitro integer,
    cedula character varying(100),
    fecha date NOT NULL,
    hora time without time zone NOT NULL,
    lugar character varying(100),
    costo money
);
    DROP TABLE public.partido;
       public         heap    potaxie_sport1    false    5            �            1259    16442    partido_partido_id_seq    SEQUENCE     �   CREATE SEQUENCE public.partido_partido_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.partido_partido_id_seq;
       public          potaxie_sport1    false    5    229            �           0    0    partido_partido_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.partido_partido_id_seq OWNED BY public.partido.partido_id;
          public          potaxie_sport1    false    230            �            1259    16443    registro_salud    TABLE     �   CREATE TABLE public.registro_salud (
    registro_salud_id integer NOT NULL,
    jugador_id integer NOT NULL,
    frecuencia_card integer NOT NULL,
    estatus character varying(100) NOT NULL,
    fecha date NOT NULL
);
 "   DROP TABLE public.registro_salud;
       public         heap    potaxie_sport1    false    5            �            1259    16446 $   registro_salud_registro_salud_id_seq    SEQUENCE     �   CREATE SEQUENCE public.registro_salud_registro_salud_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ;   DROP SEQUENCE public.registro_salud_registro_salud_id_seq;
       public          potaxie_sport1    false    231    5            �           0    0 $   registro_salud_registro_salud_id_seq    SEQUENCE OWNED BY     m   ALTER SEQUENCE public.registro_salud_registro_salud_id_seq OWNED BY public.registro_salud.registro_salud_id;
          public          potaxie_sport1    false    232            �            1259    16447    rol    TABLE     i   CREATE TABLE public.rol (
    rol_id integer NOT NULL,
    rol_nombre character varying(100) NOT NULL
);
    DROP TABLE public.rol;
       public         heap    potaxie_sport1    false    5            �            1259    16450    rol_rol_id_seq    SEQUENCE     �   CREATE SEQUENCE public.rol_rol_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.rol_rol_id_seq;
       public          potaxie_sport1    false    233    5            �           0    0    rol_rol_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.rol_rol_id_seq OWNED BY public.rol.rol_id;
          public          potaxie_sport1    false    234            �            1259    16451    torneo    TABLE     r  CREATE TABLE public.torneo (
    torneo_id integer NOT NULL,
    nombre_torneo character varying(50) NOT NULL,
    categoria_id integer NOT NULL,
    genero character varying(50) NOT NULL,
    logo character varying(100),
    usuario_admin integer NOT NULL,
    usuario_contador integer,
    usuario_doctor integer,
    fecha_inicio date NOT NULL,
    fecha_fin date
);
    DROP TABLE public.torneo;
       public         heap    potaxie_sport1    false    5            �            1259    16454    torneo_torneo_id_seq    SEQUENCE     �   CREATE SEQUENCE public.torneo_torneo_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.torneo_torneo_id_seq;
       public          potaxie_sport1    false    5    235            �           0    0    torneo_torneo_id_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public.torneo_torneo_id_seq OWNED BY public.torneo.torneo_id;
          public          potaxie_sport1    false    236            �            1259    16455    usuario    TABLE     �  CREATE TABLE public.usuario (
    usuario_id integer NOT NULL,
    nombre character varying(50) NOT NULL,
    ap_paterno character varying(50) NOT NULL,
    ap_materno character varying(50) NOT NULL,
    username character varying(50) NOT NULL,
    email character varying(100) NOT NULL,
    rol_id integer NOT NULL,
    error_autentificacion integer NOT NULL,
    password character varying
);
    DROP TABLE public.usuario;
       public         heap    potaxie_sport1    false    5            �            1259    16460    usuario_usuario_id_seq    SEQUENCE     �   CREATE SEQUENCE public.usuario_usuario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.usuario_usuario_id_seq;
       public          potaxie_sport1    false    5    237            �           0    0    usuario_usuario_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.usuario_usuario_id_seq OWNED BY public.usuario.usuario_id;
          public          potaxie_sport1    false    238            �           2604    16609    categoria categoria_id    DEFAULT     �   ALTER TABLE ONLY public.categoria ALTER COLUMN categoria_id SET DEFAULT nextval('public.categoria_categoria_id_seq'::regclass);
 E   ALTER TABLE public.categoria ALTER COLUMN categoria_id DROP DEFAULT;
       public          potaxie_sport1    false    216    215            �           2604    16610 "   disponibilidad_arbitro disp_arb_id    DEFAULT     �   ALTER TABLE ONLY public.disponibilidad_arbitro ALTER COLUMN disp_arb_id SET DEFAULT nextval('public.disponibilidad_arbitro_disp_arb_id_seq'::regclass);
 Q   ALTER TABLE public.disponibilidad_arbitro ALTER COLUMN disp_arb_id DROP DEFAULT;
       public          potaxie_sport1    false    218    217            �           2604    16611    equipo equipo_id    DEFAULT     t   ALTER TABLE ONLY public.equipo ALTER COLUMN equipo_id SET DEFAULT nextval('public.equipo_equipo_id_seq'::regclass);
 ?   ALTER TABLE public.equipo ALTER COLUMN equipo_id DROP DEFAULT;
       public          potaxie_sport1    false    220    219            �           2604    16612    jugador jugador_id    DEFAULT     x   ALTER TABLE ONLY public.jugador ALTER COLUMN jugador_id SET DEFAULT nextval('public.jugador_jugador_id_seq'::regclass);
 A   ALTER TABLE public.jugador ALTER COLUMN jugador_id DROP DEFAULT;
       public          potaxie_sport1    false    222    221            �           2604    16613 "   movimiento_economico movimiento_id    DEFAULT     �   ALTER TABLE ONLY public.movimiento_economico ALTER COLUMN movimiento_id SET DEFAULT nextval('public.movimiento_economico_movimiento_id_seq'::regclass);
 Q   ALTER TABLE public.movimiento_economico ALTER COLUMN movimiento_id DROP DEFAULT;
       public          potaxie_sport1    false    224    223            �           2604    16614    pago_partido pago_partido_id    DEFAULT     �   ALTER TABLE ONLY public.pago_partido ALTER COLUMN pago_partido_id SET DEFAULT nextval('public.pago_partido_pago_partido_id_seq'::regclass);
 K   ALTER TABLE public.pago_partido ALTER COLUMN pago_partido_id DROP DEFAULT;
       public          potaxie_sport1    false    226    225            �           2604    16615     participaciones participacion_id    DEFAULT     �   ALTER TABLE ONLY public.participaciones ALTER COLUMN participacion_id SET DEFAULT nextval('public.participaciones_participacion_id_seq'::regclass);
 O   ALTER TABLE public.participaciones ALTER COLUMN participacion_id DROP DEFAULT;
       public          potaxie_sport1    false    228    227            �           2604    16616    partido partido_id    DEFAULT     x   ALTER TABLE ONLY public.partido ALTER COLUMN partido_id SET DEFAULT nextval('public.partido_partido_id_seq'::regclass);
 A   ALTER TABLE public.partido ALTER COLUMN partido_id DROP DEFAULT;
       public          potaxie_sport1    false    230    229            �           2604    16617     registro_salud registro_salud_id    DEFAULT     �   ALTER TABLE ONLY public.registro_salud ALTER COLUMN registro_salud_id SET DEFAULT nextval('public.registro_salud_registro_salud_id_seq'::regclass);
 O   ALTER TABLE public.registro_salud ALTER COLUMN registro_salud_id DROP DEFAULT;
       public          potaxie_sport1    false    232    231            �           2604    16618 
   rol rol_id    DEFAULT     h   ALTER TABLE ONLY public.rol ALTER COLUMN rol_id SET DEFAULT nextval('public.rol_rol_id_seq'::regclass);
 9   ALTER TABLE public.rol ALTER COLUMN rol_id DROP DEFAULT;
       public          potaxie_sport1    false    234    233            �           2604    16619    torneo torneo_id    DEFAULT     t   ALTER TABLE ONLY public.torneo ALTER COLUMN torneo_id SET DEFAULT nextval('public.torneo_torneo_id_seq'::regclass);
 ?   ALTER TABLE public.torneo ALTER COLUMN torneo_id DROP DEFAULT;
       public          potaxie_sport1    false    236    235            �           2604    16620    usuario usuario_id    DEFAULT     x   ALTER TABLE ONLY public.usuario ALTER COLUMN usuario_id SET DEFAULT nextval('public.usuario_usuario_id_seq'::regclass);
 A   ALTER TABLE public.usuario ALTER COLUMN usuario_id DROP DEFAULT;
       public          potaxie_sport1    false    238    237            �          0    16411 	   categoria 
   TABLE DATA           J   COPY public.categoria (categoria_id, categoria_nombre, rango) FROM stdin;
    public          potaxie_sport1    false    215   7�       �          0    16415    disponibilidad_arbitro 
   TABLE DATA           g   COPY public.disponibilidad_arbitro (disp_arb_id, usuario_id, dia, hora_inicio, hora_final) FROM stdin;
    public          potaxie_sport1    false    217   ��       �          0    16419    equipo 
   TABLE DATA           t   COPY public.equipo (equipo_id, nombre_equipo, genero, logo, categoria_id, usuario_coach, torneo_actual) FROM stdin;
    public          potaxie_sport1    false    219   ��       �          0    16423    jugador 
   TABLE DATA           �   COPY public.jugador (jugador_id, nombre, ap_paterno, ap_materno, edad, fotografia, equipo_id, posicion, num_jugador) FROM stdin;
    public          potaxie_sport1    false    221   b�       �          0    16427    movimiento_economico 
   TABLE DATA           y   COPY public.movimiento_economico (movimiento_id, fecha, contador_id, tipo, cantidad, torneo_id, comprobante) FROM stdin;
    public          potaxie_sport1    false    223   ��       �          0    16431    pago_partido 
   TABLE DATA           s   COPY public.pago_partido (pago_partido_id, equipo_id, partido_id, completado, fecha_pago, comprobante) FROM stdin;
    public          potaxie_sport1    false    225   ��       �          0    16435    participaciones 
   TABLE DATA           Y   COPY public.participaciones (participacion_id, equipo_id, torneo_id, activo) FROM stdin;
    public          potaxie_sport1    false    227   ��       �          0    16439    partido 
   TABLE DATA           �   COPY public.partido (partido_id, torneo_id, equipo_retador, equipo_defensor, equipo_ganador, usuario_arbitro, cedula, fecha, hora, lugar, costo) FROM stdin;
    public          potaxie_sport1    false    229   �       �          0    16443    registro_salud 
   TABLE DATA           h   COPY public.registro_salud (registro_salud_id, jugador_id, frecuencia_card, estatus, fecha) FROM stdin;
    public          potaxie_sport1    false    231   ��       �          0    16447    rol 
   TABLE DATA           1   COPY public.rol (rol_id, rol_nombre) FROM stdin;
    public          potaxie_sport1    false    233   ?�       �          0    16451    torneo 
   TABLE DATA           �   COPY public.torneo (torneo_id, nombre_torneo, categoria_id, genero, logo, usuario_admin, usuario_contador, usuario_doctor, fecha_inicio, fecha_fin) FROM stdin;
    public          potaxie_sport1    false    235   ��       �          0    16455    usuario 
   TABLE DATA           �   COPY public.usuario (usuario_id, nombre, ap_paterno, ap_materno, username, email, rol_id, error_autentificacion, password) FROM stdin;
    public          potaxie_sport1    false    237   ��       �           0    0    categoria_categoria_id_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public.categoria_categoria_id_seq', 3, true);
          public          potaxie_sport1    false    216            �           0    0 &   disponibilidad_arbitro_disp_arb_id_seq    SEQUENCE SET     U   SELECT pg_catalog.setval('public.disponibilidad_arbitro_disp_arb_id_seq', 24, true);
          public          potaxie_sport1    false    218            �           0    0    equipo_equipo_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.equipo_equipo_id_seq', 36, true);
          public          potaxie_sport1    false    220            �           0    0    jugador_jugador_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.jugador_jugador_id_seq', 11, true);
          public          potaxie_sport1    false    222            �           0    0 &   movimiento_economico_movimiento_id_seq    SEQUENCE SET     U   SELECT pg_catalog.setval('public.movimiento_economico_movimiento_id_seq', 1, false);
          public          potaxie_sport1    false    224            �           0    0     pago_partido_pago_partido_id_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('public.pago_partido_pago_partido_id_seq', 1, false);
          public          potaxie_sport1    false    226            �           0    0 $   participaciones_participacion_id_seq    SEQUENCE SET     S   SELECT pg_catalog.setval('public.participaciones_participacion_id_seq', 1, false);
          public          potaxie_sport1    false    228            �           0    0    partido_partido_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.partido_partido_id_seq', 6, true);
          public          potaxie_sport1    false    230            �           0    0 $   registro_salud_registro_salud_id_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('public.registro_salud_registro_salud_id_seq', 9, true);
          public          potaxie_sport1    false    232            �           0    0    rol_rol_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.rol_rol_id_seq', 5, true);
          public          potaxie_sport1    false    234            �           0    0    torneo_torneo_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.torneo_torneo_id_seq', 1, true);
          public          potaxie_sport1    false    236            �           0    0    usuario_usuario_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.usuario_usuario_id_seq', 82, true);
          public          potaxie_sport1    false    238            �           2606    16474    categoria categoria_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.categoria
    ADD CONSTRAINT categoria_pkey PRIMARY KEY (categoria_id);
 B   ALTER TABLE ONLY public.categoria DROP CONSTRAINT categoria_pkey;
       public            potaxie_sport1    false    215            �           2606    16476 2   disponibilidad_arbitro disponibilidad_arbitro_pkey 
   CONSTRAINT     y   ALTER TABLE ONLY public.disponibilidad_arbitro
    ADD CONSTRAINT disponibilidad_arbitro_pkey PRIMARY KEY (disp_arb_id);
 \   ALTER TABLE ONLY public.disponibilidad_arbitro DROP CONSTRAINT disponibilidad_arbitro_pkey;
       public            potaxie_sport1    false    217            �           2606    16478    equipo equipo_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY public.equipo
    ADD CONSTRAINT equipo_pkey PRIMARY KEY (equipo_id);
 <   ALTER TABLE ONLY public.equipo DROP CONSTRAINT equipo_pkey;
       public            potaxie_sport1    false    219            �           2606    16480    jugador jugador_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.jugador
    ADD CONSTRAINT jugador_pkey PRIMARY KEY (jugador_id);
 >   ALTER TABLE ONLY public.jugador DROP CONSTRAINT jugador_pkey;
       public            potaxie_sport1    false    221            �           2606    16482 .   movimiento_economico movimiento_economico_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public.movimiento_economico
    ADD CONSTRAINT movimiento_economico_pkey PRIMARY KEY (movimiento_id);
 X   ALTER TABLE ONLY public.movimiento_economico DROP CONSTRAINT movimiento_economico_pkey;
       public            potaxie_sport1    false    223            �           2606    16484    pago_partido pago_partido_pkey 
   CONSTRAINT     i   ALTER TABLE ONLY public.pago_partido
    ADD CONSTRAINT pago_partido_pkey PRIMARY KEY (pago_partido_id);
 H   ALTER TABLE ONLY public.pago_partido DROP CONSTRAINT pago_partido_pkey;
       public            potaxie_sport1    false    225            �           2606    16486 $   participaciones participaciones_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public.participaciones
    ADD CONSTRAINT participaciones_pkey PRIMARY KEY (participacion_id);
 N   ALTER TABLE ONLY public.participaciones DROP CONSTRAINT participaciones_pkey;
       public            potaxie_sport1    false    227            �           2606    16488    partido partido_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_pkey PRIMARY KEY (partido_id);
 >   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_pkey;
       public            potaxie_sport1    false    229            �           2606    16490 "   registro_salud registro_salud_pkey 
   CONSTRAINT     o   ALTER TABLE ONLY public.registro_salud
    ADD CONSTRAINT registro_salud_pkey PRIMARY KEY (registro_salud_id);
 L   ALTER TABLE ONLY public.registro_salud DROP CONSTRAINT registro_salud_pkey;
       public            potaxie_sport1    false    231            �           2606    16492    rol rol_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.rol
    ADD CONSTRAINT rol_pkey PRIMARY KEY (rol_id);
 6   ALTER TABLE ONLY public.rol DROP CONSTRAINT rol_pkey;
       public            potaxie_sport1    false    233            �           2606    16494    torneo torneo_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY public.torneo
    ADD CONSTRAINT torneo_pkey PRIMARY KEY (torneo_id);
 <   ALTER TABLE ONLY public.torneo DROP CONSTRAINT torneo_pkey;
       public            potaxie_sport1    false    235            �           2606    16496    usuario usuario_email_key 
   CONSTRAINT     U   ALTER TABLE ONLY public.usuario
    ADD CONSTRAINT usuario_email_key UNIQUE (email);
 C   ALTER TABLE ONLY public.usuario DROP CONSTRAINT usuario_email_key;
       public            potaxie_sport1    false    237            �           2606    16498    usuario usuario_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.usuario
    ADD CONSTRAINT usuario_pkey PRIMARY KEY (usuario_id);
 >   ALTER TABLE ONLY public.usuario DROP CONSTRAINT usuario_pkey;
       public            potaxie_sport1    false    237            �           2606    16499 =   disponibilidad_arbitro disponibilidad_arbitro_usuario_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.disponibilidad_arbitro
    ADD CONSTRAINT disponibilidad_arbitro_usuario_id_fkey FOREIGN KEY (usuario_id) REFERENCES public.usuario(usuario_id);
 g   ALTER TABLE ONLY public.disponibilidad_arbitro DROP CONSTRAINT disponibilidad_arbitro_usuario_id_fkey;
       public          potaxie_sport1    false    237    217    3314            �           2606    16504    equipo equipo_categoria_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.equipo
    ADD CONSTRAINT equipo_categoria_id_fkey FOREIGN KEY (categoria_id) REFERENCES public.categoria(categoria_id);
 I   ALTER TABLE ONLY public.equipo DROP CONSTRAINT equipo_categoria_id_fkey;
       public          potaxie_sport1    false    215    3290    219            �           2606    16509     equipo equipo_torneo_actual_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.equipo
    ADD CONSTRAINT equipo_torneo_actual_fkey FOREIGN KEY (torneo_actual) REFERENCES public.torneo(torneo_id);
 J   ALTER TABLE ONLY public.equipo DROP CONSTRAINT equipo_torneo_actual_fkey;
       public          potaxie_sport1    false    3310    219    235            �           2606    16514     equipo equipo_usuario_coach_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.equipo
    ADD CONSTRAINT equipo_usuario_coach_fkey FOREIGN KEY (usuario_coach) REFERENCES public.usuario(usuario_id);
 J   ALTER TABLE ONLY public.equipo DROP CONSTRAINT equipo_usuario_coach_fkey;
       public          potaxie_sport1    false    237    219    3314            �           2606    16519    jugador jugador_equipo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.jugador
    ADD CONSTRAINT jugador_equipo_id_fkey FOREIGN KEY (equipo_id) REFERENCES public.equipo(equipo_id);
 H   ALTER TABLE ONLY public.jugador DROP CONSTRAINT jugador_equipo_id_fkey;
       public          potaxie_sport1    false    3294    221    219            �           2606    16524 :   movimiento_economico movimiento_economico_contador_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.movimiento_economico
    ADD CONSTRAINT movimiento_economico_contador_id_fkey FOREIGN KEY (contador_id) REFERENCES public.usuario(usuario_id);
 d   ALTER TABLE ONLY public.movimiento_economico DROP CONSTRAINT movimiento_economico_contador_id_fkey;
       public          potaxie_sport1    false    237    223    3314            �           2606    16529 8   movimiento_economico movimiento_economico_torneo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.movimiento_economico
    ADD CONSTRAINT movimiento_economico_torneo_id_fkey FOREIGN KEY (torneo_id) REFERENCES public.torneo(torneo_id);
 b   ALTER TABLE ONLY public.movimiento_economico DROP CONSTRAINT movimiento_economico_torneo_id_fkey;
       public          potaxie_sport1    false    3310    235    223            �           2606    16534 (   pago_partido pago_partido_equipo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.pago_partido
    ADD CONSTRAINT pago_partido_equipo_id_fkey FOREIGN KEY (equipo_id) REFERENCES public.equipo(equipo_id);
 R   ALTER TABLE ONLY public.pago_partido DROP CONSTRAINT pago_partido_equipo_id_fkey;
       public          potaxie_sport1    false    225    3294    219            �           2606    16539 )   pago_partido pago_partido_partido_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.pago_partido
    ADD CONSTRAINT pago_partido_partido_id_fkey FOREIGN KEY (partido_id) REFERENCES public.partido(partido_id);
 S   ALTER TABLE ONLY public.pago_partido DROP CONSTRAINT pago_partido_partido_id_fkey;
       public          potaxie_sport1    false    3304    225    229            �           2606    16544 .   participaciones participaciones_equipo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.participaciones
    ADD CONSTRAINT participaciones_equipo_id_fkey FOREIGN KEY (equipo_id) REFERENCES public.equipo(equipo_id);
 X   ALTER TABLE ONLY public.participaciones DROP CONSTRAINT participaciones_equipo_id_fkey;
       public          potaxie_sport1    false    219    3294    227            �           2606    16549 .   participaciones participaciones_torneo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.participaciones
    ADD CONSTRAINT participaciones_torneo_id_fkey FOREIGN KEY (torneo_id) REFERENCES public.torneo(torneo_id);
 X   ALTER TABLE ONLY public.participaciones DROP CONSTRAINT participaciones_torneo_id_fkey;
       public          potaxie_sport1    false    235    227    3310            �           2606    16554 $   partido partido_equipo_defensor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_equipo_defensor_fkey FOREIGN KEY (equipo_defensor) REFERENCES public.equipo(equipo_id);
 N   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_equipo_defensor_fkey;
       public          potaxie_sport1    false    229    219    3294            �           2606    16559 #   partido partido_equipo_ganador_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_equipo_ganador_fkey FOREIGN KEY (equipo_ganador) REFERENCES public.equipo(equipo_id);
 M   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_equipo_ganador_fkey;
       public          potaxie_sport1    false    219    3294    229                        2606    16564 #   partido partido_equipo_retador_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_equipo_retador_fkey FOREIGN KEY (equipo_retador) REFERENCES public.equipo(equipo_id);
 M   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_equipo_retador_fkey;
       public          potaxie_sport1    false    219    229    3294                       2606    16569    partido partido_torneo_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_torneo_id_fkey FOREIGN KEY (torneo_id) REFERENCES public.torneo(torneo_id);
 H   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_torneo_id_fkey;
       public          potaxie_sport1    false    3310    235    229                       2606    16574 $   partido partido_usuario_arbitro_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.partido
    ADD CONSTRAINT partido_usuario_arbitro_fkey FOREIGN KEY (usuario_arbitro) REFERENCES public.usuario(usuario_id);
 N   ALTER TABLE ONLY public.partido DROP CONSTRAINT partido_usuario_arbitro_fkey;
       public          potaxie_sport1    false    237    3314    229                       2606    16579 -   registro_salud registro_salud_jugador_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.registro_salud
    ADD CONSTRAINT registro_salud_jugador_id_fkey FOREIGN KEY (jugador_id) REFERENCES public.jugador(jugador_id);
 W   ALTER TABLE ONLY public.registro_salud DROP CONSTRAINT registro_salud_jugador_id_fkey;
       public          potaxie_sport1    false    3296    221    231                       2606    16584    torneo torneo_categoria_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.torneo
    ADD CONSTRAINT torneo_categoria_id_fkey FOREIGN KEY (categoria_id) REFERENCES public.categoria(categoria_id);
 I   ALTER TABLE ONLY public.torneo DROP CONSTRAINT torneo_categoria_id_fkey;
       public          potaxie_sport1    false    3290    215    235                       2606    16589     torneo torneo_usuario_admin_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.torneo
    ADD CONSTRAINT torneo_usuario_admin_fkey FOREIGN KEY (usuario_admin) REFERENCES public.usuario(usuario_id);
 J   ALTER TABLE ONLY public.torneo DROP CONSTRAINT torneo_usuario_admin_fkey;
       public          potaxie_sport1    false    237    235    3314                       2606    16594 #   torneo torneo_usuario_contador_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.torneo
    ADD CONSTRAINT torneo_usuario_contador_fkey FOREIGN KEY (usuario_contador) REFERENCES public.usuario(usuario_id);
 M   ALTER TABLE ONLY public.torneo DROP CONSTRAINT torneo_usuario_contador_fkey;
       public          potaxie_sport1    false    235    237    3314                       2606    16599 !   torneo torneo_usuario_doctor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.torneo
    ADD CONSTRAINT torneo_usuario_doctor_fkey FOREIGN KEY (usuario_doctor) REFERENCES public.usuario(usuario_id);
 K   ALTER TABLE ONLY public.torneo DROP CONSTRAINT torneo_usuario_doctor_fkey;
       public          potaxie_sport1    false    237    235    3314                       2606    16604    usuario usuario_rol_id_fkey    FK CONSTRAINT     {   ALTER TABLE ONLY public.usuario
    ADD CONSTRAINT usuario_rol_id_fkey FOREIGN KEY (rol_id) REFERENCES public.rol(rol_id);
 E   ALTER TABLE ONLY public.usuario DROP CONSTRAINT usuario_rol_id_fkey;
       public          potaxie_sport1    false    3308    233    237            =           826    16391     DEFAULT PRIVILEGES FOR SEQUENCES    DEFAULT ACL     U   ALTER DEFAULT PRIVILEGES FOR ROLE postgres GRANT ALL ON SEQUENCES TO potaxie_sport1;
                   postgres    false            ?           826    16393    DEFAULT PRIVILEGES FOR TYPES    DEFAULT ACL     Q   ALTER DEFAULT PRIVILEGES FOR ROLE postgres GRANT ALL ON TYPES TO potaxie_sport1;
                   postgres    false            >           826    16392     DEFAULT PRIVILEGES FOR FUNCTIONS    DEFAULT ACL     U   ALTER DEFAULT PRIVILEGES FOR ROLE postgres GRANT ALL ON FUNCTIONS TO potaxie_sport1;
                   postgres    false            <           826    16390    DEFAULT PRIVILEGES FOR TABLES    DEFAULT ACL     R   ALTER DEFAULT PRIVILEGES FOR ROLE postgres GRANT ALL ON TABLES TO potaxie_sport1;
                   postgres    false            �   O   x�3�tJ��J���K-�t�+)JU�P�T04TH<�1��ˈ�3/-1�$3.ohR`
U`��UZ���,o������qqq �h �      �   �   x�}�M�0���0��R~<�ѕ�+7�,H$(�ǥ��bvJ�bKMX4�f�{�$ ���֏�[���@>=�@x���D%Q�`��f����%�[!���n�_��
驩�vU>C|ߗ��-�S���'���$ek�JC]p�o��ќ<�@<G@ ��N^Ѿ�N�ֽ׫�����2��w�?�W�o�#^��
��%Uo�u�Y�h8���0����#ѻq�pD�<�j��a�}����      �   �  x�mU�r1<_�cr����<�������U�a�Y9Z�h�(��;+��E�tkF��!�{�7�f��l�h%$H��O6�jdCx�����x���6��A��ȓ��'nZK6���"t��+�&3�U��E�����u �DU���ܰ��綦B�T8���͂h��Z�J��7m���$��0%����$��wW�#NirOw߉-%��`S!��ۚm�(C/H�0���66�<D��us���b������M��t��K�~�su����8[���+��o���/t=����6y�?I[BiB˻�t��ع"+��5.���rI8�w��%,O���ǀ�WB�L�̍bK,ʶfB��7��Ј��Q(�军��`�V��m���S�є�0��[�G�:I�"�Vѕ]���&����6���r���S�Q�φM��9�x �(�a����SMѼ#XpQ\JK")F��+o���B ��ۊS���绰�`|��K�BTZt}/:f��w�\�nP*,J\�z&�� �'e7(�b��k����8*�߼�\,MQ�&�]��l�:�m���8H=X�7�P�R����D������Ei��c��H��)��K�9�7�H!�aVrY��+��8�ǎ������`;ua7��	]�:�&�[7��!���j�f�d�6��?�/6�⬕�4�t(I�E�:��?Ο2b      �   C  x�m�=n�@���� �_�Ƃ��
")Ӭ��ldvЬM�.G��p��b;�H@�S�o޾7�JY���V�z�'��(��$l�JMR�`��@k=�.QT���vĨ'"���)�1$&�to��?ی���� �1~ �b1:��ڟ�G�Q��< 1�H�L[��7lV����/ʴ��L��h\�`*f3�	>X���������fb��8ت�o��>�H�[��Kj.X���q)²���\g�-�g+��*D��9��OqRtO>�C�_���I�����oy��	�C��1R�[-�f��瓾�H�gF�����B��      �      x������ � �      �      x������ � �      �      x������ � �      �   �   x�m�=� ����&���5S�v/Y	MAb
��=�4E��W�-/�a��BUdXg؀,;�"�4��.��l�|��_\5b�(�%����	���{~�#��R5h7�SmB=ɐ�������T{E����<G��m���� ����N�b�i��Y�����l0i�!B|cCbo      �   ]   x�uͻ
�0����]*I���..u�����f�d��8̰������9h&E���קǇ�t��a\t<�����.ܦ^������{�u!��#+$      �   >   x�3�L�O.�/�2�LL����,.)JL�9���J�L����2K��L����\1z\\\ h�Q      �   ;   x�3���T)U ����Ɯa�E�y�9��������FF&���P����)W� ��      �   �  x��X�V�8]+_ы^���; ��Hx��(�b�Ȗ[����%�^��]o�cS�� ̙s8H��֕�JUeL]PW
�'���H�b��c5=�O$�9ŎQU�w�|7�������^ϸ������A�R�������p\}����q�8Z�b���_D��WD��b�,aC�0%�	#x_�_�P/#Յ]x�uE�\��0$H����r�`�VDP��h�3r�l��ÄLAH��DoP��u�v.>7,���>��@����+Z�M4S�y!i$Е��&(	s���r��Ut��	娿��9GtČ�Va^�Z_�5POH���w���J�T2���9Uaƅ����m:�!U:��r�*�S1��8�Y�
N������Yf*���:d�̪aT�����p��#�0-�V1ltAR�F�!IR�T#Rb'�q��9~u��Vl�6UK����(��+�$,��\���e�}t���X$���t�\��z�Ò�M��ug!KH
��#���2}�J�����~g�����:k���kX��"Q�H��J�\jI�)��d���VϚ�K��CR���9*����Pu��0�p�"k&I��9*�z����t(3𷿩��z �;+�׳fc��P��`z�.>g�GI��غ0�p��5�����26�:_�Y��Hg
)���y�.o���ź[�iL,Z�5,��	a�;bY��QI&�� J�sT��!�H1��^E�;`ȹ����ۄ��y#Z�{�	��7��I�̅t�<V,�H;����n���Z�R��R�)Π��\3����X����ȝP7ݼ-�I���e��FU&������n��z~�� �B�w��ܼh��`ŝ�)���jk���l��A~���	u2I��_�~�F��q;0P��o�x� ��|6q���V��O�Ʃ��y�f�^Le��F�[���	x�c5j����]�m�F��+~��a���6�%��b@]���i�k=�%7�5hVx���H�q���r6�@}��<[n�U(u��*���Ȧ��}��H!��fG�о�a��GW���G.���5\��9>���|v�d�}S�>�Y����4����>b)r6��"�,�����������q8���̝Ho4��_ ~&�����Ѱ5�����A�;�4�o�X�BÛ�Vp͗O���;�����<�Q�.�5��ܶI}�p:���:n��UG�s��nQ1��%������&c|������\nYj4>{5_��m��M�s���v�[k���l|������C/�=4qzr��v���	o�}�^ilJ�[�Y��	�|�]���Ɩ��#c�.��eF��Kk���Ir>j��׫��6K�<1�Z;t|�7C�	�6+M�Swმ�
�x�����r޹�&,�h����5��N�ӓ�I$��O,K��q�R�/4��*@�xV�T�pO$:����K��?��v�:��6b�2]����!��%QQ��v;�/\�T�t�#     