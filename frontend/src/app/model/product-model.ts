export interface Product {
  id: string;
  product_id: string;
  name: string;
  week_number: number;
  price: number;
  sales: number;
  delta_sales: number;
  currency: string;
  new_price: number;
  include_to_ad: boolean;
}
