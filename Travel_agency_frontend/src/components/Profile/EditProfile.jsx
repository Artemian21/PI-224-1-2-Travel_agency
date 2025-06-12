import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { useMutation, useQueryClient } from 'react-query'
import { userAPI } from '../../services/api'
import { useAuth } from '../../contexts/AuthContext'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import { User, Mail, Save } from 'lucide-react'
import { Link, useNavigate } from 'react-router-dom'

const EditProfile = () => {
  const { user, updateUser } = useAuth()
  const queryClient = useQueryClient()
  const navigate = useNavigate()
  const [error, setError] = useState(null)

  const { register, handleSubmit, formState: { errors } } = useForm({
    defaultValues: {
      username: user.username,
      email: user.email
    }
  })

  const mutation = useMutation(
    (data) => userAPI.update(user.id, data),
    {
      onSuccess: ({ data }) => {
        updateUser(data)
        queryClient.invalidateQueries(['user', user.id])
        navigate('/profile')
      },
      onError: (err) => {
        setError(err.response?.data?.message || 'Failed to update profile')
      }
    }
  )

  const onSubmit = (data) => mutation.mutate(data)

  return (
    <div className="max-w-md mx-auto mt-12 p-6 bg-white rounded-lg shadow-md">
      <h1 className="text-3xl font-semibold mb-8 text-center text-gray-900">Edit Profile</h1>

      {error && <ErrorMessage message={error} />}

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        <div>
          <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-2">
            Username
          </label>
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <User className="h-5 w-5 text-gray-400" />
            </div>
            <input
              id="username"
              type="text"
              {...register('username', {
                required: 'Username is required',
                minLength: { value: 3, message: 'Username must be at least 3 characters' },
                maxLength: { value: 30, message: 'Username must not exceed 30 characters' }
              })}
              placeholder="Your username"
              className={`w-full pl-10 pr-4 py-3 border rounded-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-indigo-500
                ${errors.username ? 'border-red-500' : 'border-gray-300'} transition-colors duration-200`}
            />
          </div>
          {errors.username && (
            <p className="mt-1 text-sm text-red-600">{errors.username.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
            Email
          </label>
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Mail className="h-5 w-5 text-gray-400" />
            </div>
            <input
              id="email"
              type="email"
              {...register('email', {
                required: 'Email is required',
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: 'Invalid email address'
                }
              })}
              placeholder="your.email@example.com"
              className={`w-full pl-10 pr-4 py-3 border rounded-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-indigo-500
                ${errors.email ? 'border-red-500' : 'border-gray-300'} transition-colors duration-200`}
            />
          </div>
          {errors.email && (
            <p className="mt-1 text-sm text-red-600">{errors.email.message}</p>
          )}
        </div>

        <div className="flex justify-end space-x-3">
          <Link
            to="/profile"
            className="inline-flex items-center justify-center px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-100 transition-colors duration-150"
          >
            Cancel
          </Link>

          <button
            type="submit"
            disabled={mutation.isLoading}
            className="inline-flex items-center justify-center px-5 py-2 bg-indigo-600 text-white font-semibold rounded-md hover:bg-indigo-700 disabled:bg-indigo-400 transition-colors duration-150"
          >
            {mutation.isLoading ? (
              <LoadingSpinner size="small" />
            ) : (
              <>
                <Save className="h-4 w-4 mr-2" />
                Save Changes
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  )
}

export default EditProfile