-- Replication(Redundancy)

-- => Master server(primary server)
-- => Slave server(standby server)

-- ------------------------------------------------------------------------------------------

-- STEPS TO SET UP Master and Slave server


-- initdb -D "D:/pri"
-- initdb -D "D:/sec"


-- pg_ctl -D D:\pri -o "-p 5433" -l d:\pri\logfile start 


-- psql -p 5433 -d postgres -c "CREATE ROLE replicator with REPLICATION LOGIN PASSWORD 'repl_pass';"

-- pg_basebackup -D d:\sec -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433

-- pg_ctl -D D:\sec -o "-p 5435" -l d:\sec\logfile start

-- psql -p 5433 -d postgres 

-- (In another cmd)

-- psql -p 5435 -d postgres

-- ------------------------------------------------------------------------------------------

-- 5433 - 
-- select * from pg_stat_replication;
-- 5435
-- select pg_is_in_recovery();
------------------------------------------------------------------------------------------

-- Create table in primary

CREATE TABLE test_table
(id SERIAL PRIMARY KEY, 
name TEXT);

Check in secondary

SELECT * FROM test_table;
------------------------------------------------------------------------------------------

Objective:
Create a stored procedure that inserts rental data on the primary server, and verify that changes replicate to the standby server. Add a logging mechanism to track each operation.

Tasks to Complete:
Set up streaming replication:

Primary on port 5433

Standby on port 5435

Create a table on the primary:

CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    amount NUMERIC,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
Ensure this table is replicated.

Write a stored procedure to:

Insert a new rental log entry

Accept customer_id, film_id, amount as inputs

Wrap logic in a transaction with error handling (BEGIN...EXCEPTION...END)

CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, amount)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_amount);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;

Call the procedure on the primary:

CALL sp_add_rental_log(1, 100, 4.99);

On the standby (port 5435):

Confirm that the new record appears in rental_log

Run:SELECT * FROM rental_log ORDER BY log_id DESC LIMIT 1;

Add a trigger to log any UPDATE to rental_log

CREATE TABLE rental_log_update_log (
    audit_id SERIAL PRIMARY KEY,
    field_name TEXT,
    old_value TEXT,
    new_value TEXT,
    updated_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION Update_rental_log_update_log()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD.rental_time IS DISTINCT FROM NEW.rental_time THEN
        INSERT INTO rental_log_update_log (field_name, old_value, new_value, updated_date)
        VALUES ('rental_time', OLD.rental_time::TEXT, NEW.rental_time::TEXT, CURRENT_TIMESTAMP);
    END IF;
    IF OLD.customer_id IS DISTINCT FROM NEW.customer_id THEN
        INSERT INTO rental_log_update_log (field_name, old_value, new_value, updated_date)
        VALUES ('customer_id', OLD.customer_id::TEXT, NEW.customer_id::TEXT, CURRENT_TIMESTAMP);
    END IF;
    IF OLD.film_id IS DISTINCT FROM NEW.film_id THEN
        INSERT INTO rental_log_update_log (field_name, old_value, new_value, updated_date)
        VALUES ('film_id', OLD.film_id::TEXT, NEW.film_id::TEXT, CURRENT_TIMESTAMP);
    END IF;
    IF OLD.amount IS DISTINCT FROM NEW.amount THEN
        INSERT INTO rental_log_update_log (field_name, old_value, new_value, updated_date)
        VALUES ('amount', OLD.amount::TEXT, NEW.amount::TEXT, CURRENT_TIMESTAMP);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_rental_update
AFTER UPDATE ON rental_log
FOR EACH ROW
EXECUTE FUNCTION Update_rental_log_update_log();

UPDATE rental_log
SET amount = 5.99
WHERE customer_id = 1;

