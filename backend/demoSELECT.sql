SELECT sales_week, name, price, created_on FROM priced_products join products p on priced_products.product_guid = p.id

SELECT name, week_number FROM advertised_products

SELECT name, sales_week, price FROM seasonal_products join priced_products p on seasonal_products.priced_product_guid = p.id join products pr on p.product_guid = pr.id
