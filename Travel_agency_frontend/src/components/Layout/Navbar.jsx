import React, { useState } from 'react'
import { Link, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../../contexts/AuthContext'
import {
  User,
  LogOut,
  Menu,
  X,
  Home,
  MapPin,
  Building,
  Plane,
  Settings,
  Users
} from 'lucide-react'

const Navbar = () => {
  const { user, logout, isAdmin, isManager, isAuthenticated } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/')
    setIsMobileMenuOpen(false)
  }

  const isActivePath = (path) =>
    location.pathname === path || location.pathname.startsWith(path + '/')

  const baseLinkClass =
    'flex items-center space-x-2 px-4 py-2 rounded-md text-sm font-semibold transition-colors duration-200'
  const activeLinkClass = 'text-blue-600 bg-blue-100'
  const inactiveLinkClass =
    'text-gray-700 hover:text-blue-600 hover:bg-gray-100'

  const navItems = [
    { name: 'Home', path: '/', icon: Home },
    { name: 'Tours', path: '/tours', icon: MapPin },
    { name: 'Hotels', path: '/hotels', icon: Building },
    { name: 'Transport', path: '/transport', icon: Plane }
  ]

  const authenticatedItems = [{ name: 'My Profile', path: '/profile', icon: User }]
  const managerItems = isManager ? [{ name: 'Manager Panel', path: '/manager', icon: Settings }] : []
  const adminItems = isAdmin ? [{ name: 'Admin Panel', path: '/admin', icon: Users }] : []

  const allNavItems = [
    ...navItems,
    ...(isAuthenticated ? authenticatedItems : []),
    ...managerItems,
    ...adminItems
  ]

  return (
    <nav className="bg-white shadow sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-6 sm:px-8 lg:px-10">
        <div className="flex justify-between items-center h-16">
          <Link
            to="/"
            className="text-2xl font-extrabold text-blue-600 select-none"
            aria-label="TravelApp Home"
          >
            TravelApp
          </Link>

          <div className="hidden md:flex space-x-6">
            {allNavItems.map(({ name, path, icon: Icon }) => (
              <Link
                key={path}
                to={path}
                onClick={() => setIsMobileMenuOpen(false)}
                className={`${baseLinkClass} ${
                  isActivePath(path) ? activeLinkClass : inactiveLinkClass
                }`}
              >
                <Icon className="h-5 w-5" />
                <span>{name}</span>
              </Link>
            ))}
          </div>

          <div className="hidden md:flex items-center space-x-6">
            {isAuthenticated ? (
              <>
                <div className="flex items-center space-x-3">
                  <div className="h-9 w-9 rounded-full bg-blue-600 flex items-center justify-center text-white text-lg font-semibold uppercase">
                    {user.username?.charAt(0) || 'U'}
                  </div>
                  <div className="text-gray-900 leading-tight">
                    <p className="font-semibold">{user.username}</p>
                    <p className="text-xs text-gray-500 capitalize">{user.role}</p>
                  </div>
                </div>
                <button
                  onClick={handleLogout}
                  className="flex items-center space-x-2 px-4 py-2 rounded-md text-sm font-semibold text-gray-700 hover:text-red-600 hover:bg-red-100 transition"
                  aria-label="Logout"
                >
                  <LogOut className="h-5 w-5" />
                  <span>Logout</span>
                </button>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  className="text-sm font-semibold text-gray-700 hover:text-blue-600 transition"
                >
                  Login
                </Link>
                <Link
                  to="/register"
                  className="px-5 py-2 text-sm font-semibold text-white bg-blue-600 rounded-md hover:bg-blue-700 transition"
                >
                  Register
                </Link>
              </>
            )}
          </div>

          <div className="md:hidden">
            <button
              onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              className="p-2 rounded-md text-gray-600 hover:text-gray-900 hover:bg-gray-200 transition"
              aria-label="Toggle menu"
            >
              {isMobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
            </button>
          </div>
        </div>
      </div>

      {isMobileMenuOpen && (
        <div className="md:hidden bg-white border-t border-gray-200 px-6 pb-6 space-y-3">
          {allNavItems.map(({ name, path, icon: Icon }) => (
            <Link
              key={path}
              to={path}
              onClick={() => setIsMobileMenuOpen(false)}
              className={`flex items-center space-x-3 px-4 py-3 rounded-md text-base font-semibold ${
                isActivePath(path) ? activeLinkClass : inactiveLinkClass
              }`}
            >
              <Icon className="h-6 w-6" />
              <span>{name}</span>
            </Link>
          ))}
          {isAuthenticated && (
            <button
              onClick={handleLogout}
              className="flex w-full items-center space-x-3 px-4 py-3 rounded-md text-base font-semibold text-gray-700 hover:text-red-600 hover:bg-red-100 transition"
            >
              <LogOut className="h-6 w-6" />
              <span>Logout</span>
            </button>
          )}
        </div>
      )}
    </nav>
  )
}

export default Navbar