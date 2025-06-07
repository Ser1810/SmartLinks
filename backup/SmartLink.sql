--
-- PostgreSQL database dump
--

-- Dumped from database version 15.10
-- Dumped by pg_dump version 15.10

-- Started on 2025-06-07 15:21:53

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
-- TOC entry 217 (class 1259 OID 5531157)
-- Name: History; Type: TABLE; Schema: public; Owner: puser
--

CREATE TABLE public."History" (
    "Url" character varying(250),
    "RedirectURL" character varying(250),
    "DateTime" timestamp without time zone,
    "Headers" text
);


ALTER TABLE public."History" OWNER TO puser;

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
-- TOC entry 3329 (class 0 OID 5531157)
-- Dependencies: 217
-- Data for Name: History; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."History" ("Url", "RedirectURL", "DateTime", "Headers") FROM stdin;
\.


--
-- TOC entry 3326 (class 0 OID 2256103)
-- Dependencies: 214
-- Data for Name: Rule; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."Rule" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
BrowserProcessor	http://localhost:7173/BrowserProcessor	t	{"browser": "Chrome"}	1
LanguageProcessor	http://localhost:7173/LanguageProcessor	t	{"language": "ru"}	2
TimeProcessor	http://localhost:7173/TimeProcessor	t	{"date": {"Begin": "2025-05-18 15:00:00", "End": "2025-07-18 20:00:00"}}	3
\.


--
-- TOC entry 3327 (class 0 OID 2256108)
-- Dependencies: 215
-- Data for Name: Rule2; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."Rule2" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
TimeProcessor	http://localhost:7173/TimeProcessor	t		3
TimeProcessor	http://localhost:7173/TimeProcessor	t	{	4
LanguageProcessor	http://localhost:7173/LanguageProcessor	t		1
\.


--
-- TOC entry 3328 (class 0 OID 2256113)
-- Dependencies: 216
-- Data for Name: RuleEmpty; Type: TABLE DATA; Schema: public; Owner: puser
--

COPY public."RuleEmpty" ("RuleDll", "RedirectTo", "IsActive", "Args", "Order") FROM stdin;
\.


-- Completed on 2025-06-07 15:21:53

--
-- PostgreSQL database dump complete
--

