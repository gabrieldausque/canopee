create generator gen_id_installationlogs;
SET generator gen_id_installationlogs TO 0;

CREATE TABLE INSTALLATIONLOGS
(
  ID integer NOT NULL,
  ACTIONDATE timestamp NOT NULL,
  PRODUCT varchar(255) not null,
  VERSION varchar(10) not null,
  "ACTION" varchar(255) NOT NULL,
  STATUS smallint NOT NULL,
  MESSAGE varchar(255),
  CONSTRAINT INDEX_INSTALLATIONLOGS PRIMARY KEY (ID)
);

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON INSTALLATIONLOGS TO  SYSDBA WITH GRANT OPTION;
GRANT SELECT ON INSTALLATIONLOGS TO thedoctorwho;

SET TERM ^ ;
CREATE TRIGGER INSTALLATIONLOGS_BI FOR INSTALLATIONLOGS ACTIVE
BEFORE insert POSITION 0
as 
begin
if(NEW.ID is null) then NEW.ID = gen_id(gen_id_installationlogs, 1);
end^
SET TERM ; ^


