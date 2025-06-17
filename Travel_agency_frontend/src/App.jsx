import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from 'react-query'
import { ReactQueryDevtools } from 'react-query/devtools'
import { AuthProvider } from './contexts/AuthContext'
import Layout from './components/Layout/Layout'
import ProtectedRoute, { PublicRoute } from './components/Auth/ProtectedRoute'

// Публічні компоненти
import Home from './components/Home/Home'
import Login from './components/Auth/Login'
import Register from './components/Auth/Register'
import TourList from './components/Tours/TourList'
import TourDetail from './components/Tours/TourDetail'
import HotelList from './components/Hotels/HotelList'
import HotelDetail from './components/Hotels/HotelDetail'
import RoomDetails from './components/Hotels/RoomDetails'
import TransportList from './components/Transport/TransportList'
import TransportDetail from './components/Transport/TransportDetail'
import ForbiddenPage from './components/Exeption/ForbiddenPage'
import NotFoundPage from './components/Exeption/NotFoundPage';
import ConflictPage from './components/Exeption/ConflictPage';
import ValidationErrorPage from './components/Exeption/ValidationErrorPage';
import ServerErrorPage from './components/Exeption/ServerErrorPage';

import Profile from './components/Profile/Profile'
import EditProfile from './components/Profile/EditProfile'

import ManagerPanel from './components/Manager/ManagerPanel'

import AdminPanel from './components/Admin/AdminPanel'
import UserDetails from './components/Admin/UserDetails'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
      staleTime: 5 * 60 * 1000,
    },
  },
})

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <Router>
          <Layout>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/tours" element={<TourList />} />
              <Route path="/tours/:id" element={<TourDetail />} />
              <Route path="/hotels" element={<HotelList />} />
              <Route path="/hotels/:id" element={<HotelDetail />} />
              <Route path="/rooms/:id" element={<RoomDetails />} />
              <Route path="/transport" element={<TransportList />} />
              <Route path="/transports/:id" element={<TransportDetail />} />

              <Route path="/login" element={
                <PublicRoute>
                  <Login />
                </PublicRoute>
              } />
              <Route path="/register" element={
                <PublicRoute>
                  <Register />
                </PublicRoute>
              } />

              <Route path="/profile" element={
                <ProtectedRoute>
                  <Profile />
                </ProtectedRoute>
              } />
              <Route path="/profile/edit" element={
                <ProtectedRoute>
                  <EditProfile />
                </ProtectedRoute>
              } />

              <Route path="/manager" element={
                <ProtectedRoute requireRole="Manager">
                  <ManagerPanel />
                </ProtectedRoute>
              } />

              <Route path="/admin" element={
                <ProtectedRoute requireRole="Administrator">
                  <AdminPanel />
                </ProtectedRoute>
              } />
              <Route path="/admin/users/:userId" element={
                <ProtectedRoute>
                  <UserDetails />
                </ProtectedRoute>
              } />

              <Route path="/forbidden" element={<ForbiddenPage />} />
              <Route path="/not-found" element={<NotFoundPage />} />
              <Route path="/conflict" element={<ConflictPage />} />
              <Route path="/validation-error" element={<ValidationErrorPage />} />
              <Route path="/server-error" element={<ServerErrorPage />} />
            </Routes>
          </Layout>
        </Router>
      </AuthProvider>
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  )
}

export default App
