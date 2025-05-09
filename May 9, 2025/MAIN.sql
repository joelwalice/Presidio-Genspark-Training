-- Database: dvdrental

-- DROP DATABASE IF EXISTS dvdrental;

-- CREATE DATABASE dvdrental
--     WITH
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'English_India.1252'
--     LC_CTYPE = 'English_India.1252'
--     LOCALE_PROVIDER = 'libc'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1
--     IS_TEMPLATE = False;

use dvdrental
select * from actor
--1
select title, length, rental_rate from film order by length desc

--2
select b.customer_id,count(*) as totalrental from rental a join customer b on a.customer_id=b.customer_id group by b.customer_id order by totalrental desc limit 5;

--3
select a.film_id,a.title,c.rental_id from film a left join inventory b on a.film_id=b.film_id left join rental c on b.inventory_id=c.inventory_id where c.rental_id is null

--4
select a.title,concat(c.first_name,' ',c.last_name) as fullname from film a join film_actor b on a.film_id=b.film_id join actor c on b.actor_id=c.actor_id where a.title like 'Academy Dinosaur'

--5
select count(*) as total_rental,sum(c.amount) as totalamt,a.first_name from customer a join rental b on a.customer_id=b.customer_id join payment c on a.customer_id=c.customer_id group by a.customer_id

--6
with toprentals as(
	select a.title as title,count(c.rental_id) as totalcount from film a join inventory b on a.film_id=b.film_id join rental c on b.inventory_id=c.inventory_id group by a.title
)
select title,totalcount from toprentals order by totalcount limit 3

--7
with customerrental as (
	select customer_id,count(*) as total_rental from rental group by customer_id
)
select customer_id,total_rental from customerrental where total_rental>(select avg(total_rental) from customerrental) 

--8
select * from rental;
create or replace function getrentals(c_id int)
returns int as $$
declare
total_r int;
begin 
	select count(*) into total_r from rental where customer_id=getrentals.c_id;
	return total_r;
end
$$ LANGUAGE plpgsql;

drop function getrentals(integer);

select getrentals(222);
