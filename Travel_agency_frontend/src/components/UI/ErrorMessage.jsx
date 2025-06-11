import React from 'react'
import { AlertCircle } from 'lucide-react'

const ErrorMessage = ({ message }) => (
  <div className="flex items-center space-x-2 text-red-700 bg-red-100 p-4 rounded-md shadow-sm">
    <AlertCircle className="h-5 w-5 flex-shrink-0" />
    <span className="font-medium">{message}</span>
  </div>
)

export default ErrorMessage