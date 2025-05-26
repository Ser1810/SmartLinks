--
-- PostgreSQL database dump
--

-- Dumped from database version 15.10
-- Dumped by pg_dump version 15.10

-- Started on 2025-05-26 09:53:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 214 (class 1259 OID 2256103)
-- Name: Rule; Type: TABLE; Schema: public; Owner: puser
--

CREATE TABLE public."Rule" (
    "RuleDll" character varying(250),
    "RedirectTo" character varying(250),
    "IsActive" boolean,
    "Args" text,
    "Order" integer
);


ALTER TABLE public."Rule" OWNER TO puser;

--
-- TOC entry 215 (class 1259 OID 2256108)
-- Name: Rule2; Type: TABLE; Schema: public; Owner: puser
--

CREATE TABLE public."Rule2" (
    "RuleDll" character varying(250),
    "RedirectTo" character varying(250),
    "IsActive" boolean,
    "Args" text,
    "Order" integer
);


ALTER TABLE public."Rule2" OWNER TO puser;

--
-- TOC entry 216 (class 1259 OID 2256113)
-- Name: RuleEmpty; Type: TABLE; Schema: public; Owner: puser
--

CREATE TABLE public."RuleEmpty" (
    "RuleDll" character varying(250),
    "RedirectTo" character varying(250),
    "IsActive" boolean,
    "Args" text,
    "Order" integer
);


ALTER TABLE public."RuleEmpty" OWNER TO puser;

--
-- TOC entry 3322 (class 0 OID 2256103)
-- Dependencies: 214
-- Data for Name: Rule; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."Rule" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
BrowserProcessor	http://localhost:7173/BrowserProcessor	t	{"browser": "Chrome"}	1
BrowserProcessor	http://localhost:7173/BrowserProcessor	t		1
LanguageProcessor	http://localhost:7173/LanguageProcessor	t	{"language": "ru"}	2
LanguageProcessor	http://localhost:7173/LanguageProcessor	t		2
TimeProcessor	http://localhost:7173/TimeProcessor	t	{"date": {"Begin": "2025-05-18 15:00:00", "End": "2025-07-18 20:00:00"}}	3
\.


--
-- TOC entry 3323 (class 0 OID 2256108)
-- Dependencies: 215
-- Data for Name: Rule2; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."Rule2" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
TimeProcessor	http://localhost:7173/TimeProcessor	t		3
TimeProcessor	http://localhost:7173/TimeProcessor	t	{	4
\.


--
-- TOC entry 3324 (class 0 OID 2256113)
-- Dependencies: 216
-- Data for Name: RuleEmpty; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."RuleEmpty" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
\.


-- Completed on 2025-05-26 09:53:35

--
-- PostgreSQL database dump complete
--

