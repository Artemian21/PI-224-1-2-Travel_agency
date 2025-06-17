import React from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from '../../contexts/AuthContext'
import LoadingSpinner from '../UI/LoadingSpinner'

const ProtectedRoute = ({ children, requireAuth = true, requireRole, requirePermission, redirectTo = '/login' }) => {
  const { user, isLoading } = useAuth()
  const location = useLocation()

  if (isLoading) {
    return <LoadingSpinner />
  }

  if (requireAuth && !user) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />
  }

  if (requireRole) {
    const roleHierarchy = {
      'Registered': 1,
      'Manager': 2,
      'Administrator': 3,
    }

    const userLevel = roleHierarchy[user?.role] || 0
    const requiredLevel = roleHierarchy[requireRole] || 0

    if (userLevel < requiredLevel) {
      return <Navigate to="/forbidden" replace />
    }
  }

  if (requirePermission && typeof requirePermission === 'function') {
    if (!requirePermission(user)) {
      return <Navigate to="/forbidden" replace />
    }
  }

  return children
}

export const PublicRoute = ({ children, redirectTo = '/' }) => {
  const { user, isLoading } = useAuth()

  if (isLoading) {
    return <LoadingSpinner />
  }

  if (user) {
    return <Navigate to={redirectTo} replace />
  }

  return children
}

export default ProtectedRoute