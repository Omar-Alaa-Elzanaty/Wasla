select Brand,count(*)as total_vehicles 
from Vehicles
group by Brand