select Category,Brand,Capcity,count(*)as total_vehicles 
from Vehicles
group by Category,Brand,Capcity