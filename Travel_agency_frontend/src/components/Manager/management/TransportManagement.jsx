import React from 'react'
import { Plus, Edit, Trash2, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'
import DeleteConfirmationModal from '../../UI/DeleteConfirmationModal'
import { useManageTransport } from '../../hooks/useManageTransport'

const TransportManagement = () => {
  const {
    loading,
    error,
    formData,
    isEditing,
    deleteTransportId,
    filteredTransports,
    searchTerm,
    setDeleteTransportId,
    confirmDelete,
    cancelDelete,
    setSearchTerm,
    handleChange,
    handleSubmit,
    handleEdit,
    handleCancel,
  } = useManageTransport()

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">Failed to load transports</p>

  return (
    <div className="space-y-6 relative">
      <div className="flex justify-between items-center">
        <h2 className="text-xl font-semibold">Manage Transport</h2>
        <button
          onClick={() => handleEdit('new')}
          className="btn-primary flex items-center gap-2"
        >
          <Plus className="h-4 w-4" />
          <span>Add New Transport</span>
        </button>
      </div>

      <input
        type="text"
        placeholder="Search transports..."
        className="form-input w-full max-w-md"
        value={searchTerm}
        onChange={e => setSearchTerm(e.target.value)}
      />

      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              {['Type', 'Company', 'Price', 'Departure date', 'Arrival date', 'Actions'].map(header => (
                <th
                  key={header}
                  className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                >
                  {header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {filteredTransports?.map(transport => (
              <tr key={transport.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 capitalize">{transport.type}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{transport.company}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${transport.price}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {new Date(transport.departureDate).toLocaleString('en-US', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                  })}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {new Date(transport.arrivalDate).toLocaleString('en-US', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                  })}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <div className="flex space-x-3">
                    <Link to={`/transports/${transport.id}`} className="text-blue-600 hover:text-blue-900">
                      <Eye className="h-4 w-4" />
                    </Link>
                    <button onClick={() => handleEdit(transport)} className="text-green-600 hover:text-green-900">
                      <Edit className="h-4 w-4" />
                    </button>
                    <button onClick={() => setDeleteTransportId(transport.id)} className="text-red-600 hover:text-red-900">
                      <Trash2 className="h-4 w-4" />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {deleteTransportId && (
        <DeleteConfirmationModal onConfirm={confirmDelete} onCancel={cancelDelete}>
          Are you sure you want to delete this transport?
        </DeleteConfirmationModal>
      )}

      {isEditing && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
          onClick={handleCancel}
        >
          <div className="bg-white rounded-md p-6 max-w-md w-full" onClick={e => e.stopPropagation()}>
            <h3 className="text-lg font-semibold mb-4">
              {formData.id ? 'Edit Transport' : 'Create New Transport'}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <input
                type="text"
                name="type"
                placeholder="Type"
                value={formData.type || ''}
                onChange={handleChange}
                required
                className="w-full p-2 border rounded"
              />
              <input
                type="text"
                name="company"
                placeholder="Company"
                value={formData.company || ''}
                onChange={handleChange}
                required
                className="w-full p-2 border rounded"
              />
              <input
                type="number"
                name="price"
                placeholder="Price"
                value={formData.price || ''}
                onChange={handleChange}
                required
                min="0"
                className="w-full p-2 border rounded"
              />
              <label className="block">
                Departure Date:
                <input
                  type="datetime-local"
                  name="departureDate"
                  value={formData.departureDate || ''}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border rounded mt-1"
                />
              </label>
              <label className="block">
                Arrival Date:
                <input
                  type="datetime-local"
                  name="arrivalDate"
                  value={formData.arrivalDate || ''}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border rounded mt-1"
                />
              </label>

              <div className="flex justify-end space-x-3">
                <button type="button" onClick={handleCancel} className="btn-secondary">
                  Cancel
                </button>
                <button type="submit" className="btn-primary">
                  Save
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default TransportManagement