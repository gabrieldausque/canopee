CREATE TABLE USERINFOS
(
  ID integer NOT NULL,
  RPPS varchar(255) NOT NULL,
  SAPID varchar(255),
  CONSTRAINT USERINFOS_PK PRIMARY KEY (ID)
);

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON USERINFOS TO  SYSDBA WITH GRANT OPTION;

GRANT SELECT
 ON USERINFOS TO  THEDOCTORWHO;




