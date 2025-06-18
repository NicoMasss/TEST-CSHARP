--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.2

-- Started on 2025-06-18 00:42:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE "Project_Engenharia";
--
-- TOC entry 4881 (class 1262 OID 24642)
-- Name: Project_Engenharia; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "Project_Engenharia" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Portuguese_Brazil.1252';


ALTER DATABASE "Project_Engenharia" OWNER TO postgres;

\connect "Project_Engenharia"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 854 (class 1247 OID 32911)
-- Name: task_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.task_status AS ENUM (
    'pendente',
    'atrasado',
    'concluido',
    'em_andamento'
);


ALTER TYPE public.task_status OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 32874)
-- Name: admin_users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin_users (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    name character varying(100) NOT NULL,
    email character varying(150) NOT NULL,
    password_hash character varying(255) NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    is_deleted boolean DEFAULT false
);


ALTER TABLE public.admin_users OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 32864)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    name character varying(100) NOT NULL,
    email character varying(150) NOT NULL,
    is_deleted boolean DEFAULT false,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    description text
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 41061)
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    title character varying(150) NOT NULL,
    description text,
    status character varying(50) DEFAULT 'pendente'::character varying,
    assigned_to uuid,
    is_deleted boolean DEFAULT false,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    due_date date,
    assigned_to_clients uuid,
    CONSTRAINT chk_status_valid CHECK (((status)::text = ANY (ARRAY[('pendente'::character varying)::text, ('atrasado'::character varying)::text, ('concluido'::character varying)::text, ('em_andamento'::character varying)::text])))
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- TOC entry 4874 (class 0 OID 32874)
-- Dependencies: 218
-- Data for Name: admin_users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.admin_users VALUES ('393dcf6f-28cb-4baf-8842-b0b34941260f', 'teste', 'teste1@gmail.com', '$2a$11$ynW9/QAWHH3Sm9CwfFmAZ.Ux/FD9XspiPEPwBU/D0dNyz0AG.eJRq', '2025-05-26 15:44:52.856392', false);
INSERT INTO public.admin_users VALUES ('e307d2be-a84c-4629-8dec-e714d6e8bb96', 'teste', 'teste@gmail.com', '$2a$11$oQWJ2zvTGwlwdmhtJEemh.gp2OqEnLUCGfdAAFXz6leCHBTKNZpgu', '2025-05-26 15:54:25.39012', false);
INSERT INTO public.admin_users VALUES ('b9855df8-b603-49a9-9b2c-447bf237c06f', 'teste', 'testeaa@gmail.com', '$2a$11$jORcZBgpohn/HBM8SD6CnOzy7ntLznVT7mSau05xwTSTiz9A0bGcW', '2025-05-26 15:56:42.850272', false);
INSERT INTO public.admin_users VALUES ('ff66f86c-dd2d-41d5-8da7-3d9bd6286723', 'teste2', 'teste2@gmail.com', '$2a$11$bhwFvDKb/ofqiH7WwmvcUO6Ls3geEX0Bp2Lekg3INiP8EFtMo/JVS', '2025-06-04 15:11:50.723628', false);


--
-- TOC entry 4873 (class 0 OID 32864)
-- Dependencies: 217
-- Data for Name: clients; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.clients VALUES ('afa0dba3-b4d8-4566-9d3e-4a74d0590a35', 'nicolas', 'nicolas@gmail.com', false, '2025-06-05 15:36:05.999799', 'trabalha bastante');
INSERT INTO public.clients VALUES ('1fa675b7-d6a2-43c0-8aec-d0e7c4529ce5', 'nicolas', 'nickmassochin@gmail.com', true, '2025-05-26 15:59:05.736071', 'trabalha com tudo');
INSERT INTO public.clients VALUES ('74f823ef-3e70-4581-9281-65ab601d20c9', 'teste', 'teste@gmail.com', false, '2025-06-16 10:28:29.321124', 'teste');


--
-- TOC entry 4875 (class 0 OID 41061)
-- Dependencies: 219
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.tasks VALUES ('938e6eb1-6add-46a0-a722-e51388f0ba15', 'teste1', 'teste1', 'em_andamento', 'e307d2be-a84c-4629-8dec-e714d6e8bb96', false, '2025-06-05 15:35:40.357963', '2025-06-05', 'afa0dba3-b4d8-4566-9d3e-4a74d0590a35');
INSERT INTO public.tasks VALUES ('c6107463-648d-49d1-91b2-adccc4a4581f', 'tarefa foda', 'mt foda', 'em_andamento', 'ff66f86c-dd2d-41d5-8da7-3d9bd6286723', false, '2025-06-11 15:34:23.083219', NULL, NULL);
INSERT INTO public.tasks VALUES ('22956f0a-aefe-40f9-954d-0be0e2d5b547', 'eu', 't', 'em_andamento', '393dcf6f-28cb-4baf-8842-b0b34941260f', false, '2025-06-15 21:12:02.256203', '2025-07-30', NULL);
INSERT INTO public.tasks VALUES ('1707f055-961d-400a-b717-3bfa5fa2a9ee', 'a', 'a', 'em_andamento', '393dcf6f-28cb-4baf-8842-b0b34941260f', false, '2025-06-15 21:14:06.928136', '2025-07-30', NULL);
INSERT INTO public.tasks VALUES ('df217f18-bbd4-424c-8e70-7f0644187435', 'aa', 'aa', 'em_andamento', '393dcf6f-28cb-4baf-8842-b0b34941260f', false, '2025-06-15 21:17:23.268231', '2025-07-30', 'afa0dba3-b4d8-4566-9d3e-4a74d0590a35');
INSERT INTO public.tasks VALUES ('4c987c75-42ea-4c7a-a102-c95596458955', 'teste', 'teste', 'pendente', '393dcf6f-28cb-4baf-8842-b0b34941260f', false, '2025-06-15 21:32:12.531683', '2025-07-30', 'afa0dba3-b4d8-4566-9d3e-4a74d0590a35');


--
-- TOC entry 4721 (class 2606 OID 32884)
-- Name: admin_users admin_users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users
    ADD CONSTRAINT admin_users_email_key UNIQUE (email);


--
-- TOC entry 4723 (class 2606 OID 32882)
-- Name: admin_users admin_users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin_users
    ADD CONSTRAINT admin_users_pkey PRIMARY KEY (id);


--
-- TOC entry 4717 (class 2606 OID 32873)
-- Name: clients clients_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_email_key UNIQUE (email);


--
-- TOC entry 4719 (class 2606 OID 32871)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (id);


--
-- TOC entry 4725 (class 2606 OID 41072)
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (id);


--
-- TOC entry 4726 (class 2606 OID 41073)
-- Name: tasks fk_assigned_to_clients; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT fk_assigned_to_clients FOREIGN KEY (assigned_to_clients) REFERENCES public.clients(id) ON DELETE SET NULL;


--
-- TOC entry 4727 (class 2606 OID 41078)
-- Name: tasks tasks_assigned_to_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_assigned_to_fkey FOREIGN KEY (assigned_to) REFERENCES public.admin_users(id);


-- Completed on 2025-06-18 00:42:35

--
-- PostgreSQL database dump complete
--

