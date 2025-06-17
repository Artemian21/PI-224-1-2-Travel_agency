import { format } from 'date-fns'

export const formatDate = (date) => {
  if (!date) return ''
  return format(new Date(date), 'dd/MM/yyyy')
}

export const formatDateTime = (date) => {
  if (!date) return ''
  return format(new Date(date), 'dd/MM/yyyy HH:mm')
}

export const formatPrice = (price) => {
  if (!price) return '0'
  return new Intl.NumberFormat('uk-UA', {
    style: 'currency',
    currency: 'UAH'
  }).format(price)
}