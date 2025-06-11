import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../../contexts/AuthContext'

const Login = () => {
  const { register, handleSubmit, formState: { errors } } = useForm()
  const { login } = useAuth()
  const navigate = useNavigate()
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')

  const onSubmit = async (data) => {
    setIsLoading(true)
    setError('')

    const result = await login(data.email, data.password)

    if (result.success) {
      navigate('/')
    } else {
      setError(result.error)
    }

    setIsLoading(false)
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8 space-y-6">
        <h2 className="text-3xl font-semibold text-center text-gray-800">Sign in to your account</h2>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input
              type="email"
              className="w-full px-4 py-2 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              {...register('email', {
                required: 'Email is required',
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: 'Invalid email address'
                }
              })}
            />
            {errors.email && <p className="text-sm text-red-600 mt-1">{errors.email.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Password</label>
            <input
              type="password"
              className="w-full px-4 py-2 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              {...register('password', { required: 'Password is required' })}
            />
            {errors.password && <p className="text-sm text-red-600 mt-1">{errors.password.message}</p>}
          </div>

          {error && <p className="text-sm text-red-600 text-center">{error}</p>}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full py-2 rounded-xl bg-blue-600 text-white font-semibold hover:bg-blue-700 transition disabled:opacity-50"
          >
            {isLoading ? 'Loading...' : 'Sign In'}
          </button>
        </form>

        <p className="text-center text-sm text-gray-600">
          Don't have an account?{' '}
          <Link to="/register" className="text-blue-600 hover:underline font-medium">
            Register
          </Link>
        </p>
      </div>
    </div>
  )
}

export default Login