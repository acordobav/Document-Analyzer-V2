-- Creates the database
CREATE DATABASE docanalyzer ENCODING 'UTF8';

-- Connects to the database 
\c docanalyzer

-- Creates the tables
CREATE TABLE EMPLOYEE (
	employee_id serial PRIMARY KEY,
	full_name VARCHAR (100) NOT NULL
);

CREATE TABLE EMPLOYEE_REFERENCE_BY_DOCUMENT (
	employee_id INT,
	CONSTRAINT fk_employee_id FOREIGN KEY (employee_id) REFERENCES EMPLOYEE(employee_id),
	document_id VARCHAR(50) NOT NULL,
	ocurrences INT DEFAULT 0,
	PRIMARY KEY (employee_id, document_id)
);

-- Populates the database
INSERT INTO EMPLOYEE (full_name) VALUES ('Jose Montoya');
INSERT INTO EMPLOYEE (full_name) VALUES ('Fabian Gonzalez');
INSERT INTO EMPLOYEE (full_name) VALUES ('Marcelo Sanchez');
INSERT INTO EMPLOYEE (full_name) VALUES ('Arturo Cordoba');
INSERT INTO EMPLOYEE (full_name) VALUES ('Isaac Ramirez');