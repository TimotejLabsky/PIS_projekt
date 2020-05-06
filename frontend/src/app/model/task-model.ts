export enum TaskType {
  price_update= 'price-update',
  ordering_cancellation = 'ordering-cancellation',
  advertisement_picking = 'advertisement-picking',
  include_to_season = 'not_implemented_yet'
}

export interface Task {
  taskType: TaskType
}
