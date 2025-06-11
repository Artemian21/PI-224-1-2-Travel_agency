import React, { createContext, useState, useContext, useEffect } from 'react'
import { apiClient } from '../services/api'

const AuthContext = createContext()

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const initializeAuth = () => {
      const token = localStorage.getItem('token')
      const storedUser = localStorage.getItem('user')

      if (token && storedUser) {
        apiClient.defaults.headers.common['Authorization'] = `Bearer ${token}`
        setUser({ ...JSON.parse(storedUser), token })
      }

      setIsLoading(false)
    }
    initializeAuth()
  }, [])

  const login = async (email, password) => {
    try {
      const response = await apiClient.post('/api/Auth/login', { email, password })
      const { token, user } = response.data

      localStorage.setItem('token', token)
      localStorage.setItem('user', JSON.stringify(user))
      apiClient.defaults.headers.common['Authorization'] = `Bearer ${token}`

      setUser({ ...user, token })
      return { success: true }
    } catch (error) {
      return { success: false, error: error.response?.data?.message || 'Login failed' }
    }
  }

  const register = async (username, email, password) => {
    try {
      await apiClient.post('/api/Auth/register', { username, email, password })
      return { success: true }
    } catch (error) {
      return { success: false, error: error.response?.data?.message || 'Registration failed' }
    }
  }

  const logout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    delete apiClient.defaults.headers.common['Authorization']
    setUser(null)
  }

  const updateUser = (updatedUser) => {
    setUser(prev => ({ ...prev, ...updatedUser }))
    localStorage.setItem('user', JSON.stringify({ ...user, ...updatedUser }))
  }

  const isGuest = !user
  const isUser = user?.role === 'Registered'
  const isManager = user?.role === 'Manager'
  const isAdmin = user?.role === 'Administrator'

  const value = {
    user,
    login,
    register,
    logout,
    updateUser,
    isLoading,
    isAuthenticated: !!user,

    isGuest: () => isGuest,
    isUser: () => isUser,
    isManager: () => isManager,
    isAdmin: () => isAdmin,

    canViewAllUsers: () => isAdmin,
    canEditUserRole: () => isAdmin,
    canDeleteUser: (targetUserId) => isAdmin && user?.id !== targetUserId,
    canViewUserProfile: (targetUserId) => isAdmin || isManager || user?.id === targetUserId,
    canEditUserProfile: (targetUserId) => isAdmin || user?.id === targetUserId,

    canMakeBooking: () => isUser || isManager || isAdmin,
    canViewOwnBookings: () => isUser || isManager || isAdmin,
    canCancelOwnBooking: (bookingUserId) =>
      (isUser || isManager || isAdmin) && user?.id === bookingUserId,
    canViewAllBookings: () => isManager || isAdmin,
    canManageBookingStatus: () => isManager || isAdmin,

    canManageTours: () => isManager || isAdmin,
    canManageHotels: () => isManager || isAdmin,
    canManageRooms: () => isManager || isAdmin,
    canManageTransport: () => isManager || isAdmin,

    canViewPublicContent: true,
    canRegister: () => isGuest,
    canLogin: () => isGuest,
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}