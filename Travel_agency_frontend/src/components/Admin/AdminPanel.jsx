import React, { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from 'react-query'
import { userAPI } from '../../services/api'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import DeleteConfirmationModal from '../UI/DeleteConfirmationModal'
import { User, Edit, Trash2, Eye, Search, Shield } from 'lucide-react'
import { useAuth } from '../../contexts/AuthContext'
import { Link } from 'react-router-dom'

const AdminPanel = () => {
  const { 
    user: currentUser, 
    canViewAdminPanel = true, 
    canSearchAllUsers = true, 
    canEditUserRoles = true, 
    canDeleteUsers = true 
  } = useAuth()
  const queryClient = useQueryClient()
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedRole, setSelectedRole] = useState('All')
  const [deleteModalUserId, setDeleteModalUserId] = useState(null)
  const [editModalUser, setEditModalUser] = useState(null)
  const [editForm, setEditForm] = useState({ username: '', email: '' })

  if (!canViewAdminPanel) {
    return <ErrorMessage message="Access denied. Administrator privileges required." />
  }

  const { data: users, isLoading, error } = useQuery(
    ['users'], 
    () => userAPI.getAll(), 
    {
      select: (data) => data.data,
      enabled: canSearchAllUsers
    }
  )

  const updateUserMutation = useMutation(
    ({ userId, updatedData }) => userAPI.update(userId, updatedData), 
    {
      onSuccess: () => {
        queryClient.invalidateQueries(['users'])
        setEditModalUser(null)
      }
    }
  )

  const updateRoleMutation = useMutation(
    ({ userId, role }) => userAPI.updateRole(userId, role), 
    {
      onSuccess: () => queryClient.invalidateQueries(['users'])
    }
  )

  const deleteUserMutation = useMutation(
    userId => userAPI.delete(userId), 
    {
      onSuccess: () => {
        queryClient.invalidateQueries(['users'])
        setDeleteModalUserId(null)
      }
    }
  )

  const filteredUsers = users?.filter(user => {
    const matchesSearch = 
      user.username.toLowerCase().includes(searchTerm.toLowerCase()) || 
      user.email.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesRole = selectedRole === 'All' || user.role === selectedRole
    const notCurrentUser = user.id !== currentUser.id
    return matchesSearch && matchesRole && notCurrentUser
  })

  const openEditModal = (user) => {
    setEditForm({ username: user.username, email: user.email })
    setEditModalUser(user)
  }

  const handleEditSubmit = () => {
    if (editModalUser) {
      updateUserMutation.mutate({ userId: editModalUser.id, updatedData: editForm })
    }
  }

  if (isLoading) return <LoadingSpinner />
  if (error) return <ErrorMessage message="Failed to load users" />

  return (
    <div className="space-y-6 p-6">
      <div className="flex justify-between items-center">
        <div className="flex items-center gap-3">
          <Shield className="h-8 w-8 text-red-600" />
          <h1 className="text-3xl font-bold text-gray-800">Admin Panel</h1>
        </div>
        <div className="text-sm text-red-700 bg-red-100 px-4 py-1 rounded-full">
          Total users: {users?.length || 0}
        </div>
      </div>

      <div className="bg-white p-6 rounded-2xl shadow-lg space-y-6">
        <div className="flex flex-col md:flex-row gap-4">
          <div className="relative w-full">
            <Search className="absolute left-3 top-3 h-5 w-5 text-gray-400" />
            <input
              type="text"
              placeholder="Search by username or email..."
              className="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
              disabled={!canSearchAllUsers}
            />
          </div>
          <select
            value={selectedRole}
            onChange={e => setSelectedRole(e.target.value)}
            className="px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="All">All Roles</option>
            <option value="Registered">Registered</option>
            <option value="Manager">Manager</option>
            <option value="Administrator">Administrator</option>
          </select>
        </div>

        <div className="overflow-x-auto">
          <table className="min-w-full table-auto border border-gray-200 rounded-lg">
            <thead className="bg-gray-100 text-gray-600 text-sm">
              <tr>
                <th className="px-6 py-3 text-left font-medium">User</th>
                <th className="px-6 py-3 text-left font-medium">Email</th>
                <th className="px-6 py-3 text-left font-medium">Role</th>
                <th className="px-6 py-3 text-left font-medium">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white text-sm text-gray-700 divide-y divide-gray-100">
              {filteredUsers?.map(user => (
                <tr key={user.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center gap-3">
                      <div className="h-10 w-10 rounded-full bg-gray-200 flex items-center justify-center">
                        <User className="h-5 w-5 text-gray-500" />
                      </div>
                      <div>
                        <div className="font-semibold">{user.username}</div>
                        <div className="text-gray-400 text-xs">ID: {user.id}</div>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">{user.email}</td>
                  <td className="px-6 py-4">
                    <select
                      value={user.role}
                      onChange={e => updateRoleMutation.mutate({ userId: user.id, role: e.target.value })}
                      className="border border-gray-300 rounded px-2 py-1 text-sm"
                      disabled={!canEditUserRoles}
                    >
                      <option value="Registered">Registered</option>
                      <option value="Manager">Manager</option>
                      <option value="Administrator">Administrator</option>
                    </select>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex gap-3">
                      <Link
                        to={`/admin/users/${user.id}`}
                        className="text-blue-600 hover:text-blue-800 transition"
                        title="View Profile"
                      >
                        <Eye className="w-4 h-4" />
                      </Link>
                      <button
                        onClick={() => openEditModal(user)}
                        className="text-green-600 hover:text-green-800 transition"
                        title="Edit User"
                      >
                        <Edit className="w-4 h-4" />
                      </button>
                      <button
                        onClick={() => setDeleteModalUserId(user.id)}
                        className="text-red-600 hover:text-red-800 transition"
                        title="Delete User"
                        disabled={!canDeleteUsers}
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {filteredUsers?.length === 0 && (
            <div className="text-center text-gray-500 py-6">No users found.</div>
          )}
        </div>
      </div>

      {deleteModalUserId && (
        <DeleteConfirmationModal
          title="Confirm Deletion"
          onCancel={() => setDeleteModalUserId(null)}
          onConfirm={() => deleteUserMutation.mutate(deleteModalUserId)}
        >
          Are you sure you want to delete this user? This will also delete all their bookings.
        </DeleteConfirmationModal>
      )}

      {editModalUser && (
        <Modal
          title="Edit User"
          onCancel={() => setEditModalUser(null)}
          onConfirm={handleEditSubmit}
        >
          <div className="space-y-4">
            <input
              type="text"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={editForm.username}
              onChange={e => setEditForm({ ...editForm, username: e.target.value })}
              placeholder="Username"
            />
            <input
              type="email"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={editForm.email}
              onChange={e => setEditForm({ ...editForm, email: e.target.value })}
              placeholder="Email"
            />
          </div>
        </Modal>
      )}
    </div>
  )
}

const Modal = ({ title, children, onCancel, onConfirm }) => (
  <div
    className="fixed inset-0 bg-black/40 backdrop-blur-sm flex items-center justify-center z-50"
    onClick={onCancel}
  >
    <div
      className="bg-white rounded-2xl shadow-2xl p-6 w-full max-w-lg transition-all duration-300 scale-100"
      onClick={e => e.stopPropagation()}
    >
      <h3 className="text-xl font-bold text-gray-800 mb-4">{title}</h3>
      <div className="text-gray-700 mb-6">{children}</div>
      <div className="flex justify-end space-x-3">
        <button
          onClick={onCancel}
          className="px-4 py-2 rounded-xl border border-gray-300 text-gray-600 hover:bg-gray-100 transition"
        >
          Cancel
        </button>
        <button
          onClick={onConfirm}
          className="px-4 py-2 rounded-xl bg-blue-600 text-white hover:bg-blue-700 transition"
        >
          Confirm
        </button>
      </div>
    </div>
  </div>
)

export default AdminPanel