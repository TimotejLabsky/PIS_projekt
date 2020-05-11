SELECT sales_week, name, price, created_on FROM priced_products join products p on priced_products.product_guid = p.id

SELECT name, week_number FROM advertised_products
