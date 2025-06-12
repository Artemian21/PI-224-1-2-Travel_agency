import React from 'react'
import { Plus, Edit, Trash2, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'
import DeleteConfirmationModal from '../../UI/DeleteConfirmationModal'
import { useManageHotels } from '../../hooks/useManageHotels'

const HotelManagement = () => {
  const {
    hotels,
    searchTerm,
    setSearchTerm,
    isLoading,
    editingHotel,
    setEditingHotel,
    form,
    onChangeForm,
    onSubmit,
    onEdit,
    handleDelete,
    deleteHotelId,
    confirmDelete,
    cancelDelete,
    createOrUpdateLoading,
    resetForm,
    error,
    filteredHotels,
  } = useManageHotels()

  if (isLoading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">Failed to load hotels</p>

  return (
    <div className="space-y-6 relative">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-semibold">Manage Hotels</h2>
        <button
          onClick={() => {
            setEditingHotel('new')
            resetForm()
          }}
          className="btn-primary flex items-center gap-2 px-4 py-2 text-sm"
        >
          <Plus className="h-5 w-5" />
          <span>Add New Hotel</span>
        </button>
      </div>

      <input
        type="text"
        placeholder="Search hotels..."
        className="form-input max-w-md w-full px-3 py-2 rounded border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />

      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50 sticky top-0 z-10">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wide">
                Hotel
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wide">
                Location
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase tracking-wide">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {filteredHotels?.length === 0 ? (
              <tr>
                <td colSpan={3} className="py-6 text-center text-gray-500">
                  No hotels found
                </td>
              </tr>
            ) : (
              filteredHotels.map((hotel) => (
                <tr key={hotel.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {hotel.name}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    {hotel.country}, {hotel.city}, {hotel.address}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium flex space-x-4">
                    <Link
                      to={`/hotels/${hotel.id}`}
                      className="text-blue-600 hover:text-blue-900"
                      aria-label={`View details of ${hotel.name}`}
                    >
                      <Eye className="h-5 w-5" />
                    </Link>
                    <button
                      onClick={() => onEdit(hotel)}
                      className="text-green-600 hover:text-green-900"
                      aria-label={`Edit ${hotel.name}`}
                    >
                      <Edit className="h-5 w-5" />
                    </button>
                    <button
                      onClick={() => handleDelete(hotel.id)}
                      className="text-red-600 hover:text-red-900"
                      aria-label={`Delete ${hotel.name}`}
                    >
                      <Trash2 className="h-5 w-5" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {deleteHotelId && (
        <DeleteConfirmationModal onConfirm={confirmDelete} onCancel={cancelDelete}>
          Are you sure you want to delete this hotel?
        </DeleteConfirmationModal>
      )}

      {(editingHotel === 'new' || editingHotel) && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
          onClick={() => {
            setEditingHotel(null)
            resetForm()
          }}
        >
          <div
            className="bg-white rounded-md p-6 max-w-md w-full shadow-lg"
            onClick={(e) => e.stopPropagation()}
          >
            <h3 className="text-lg font-semibold mb-5">
              {editingHotel === 'new' ? 'Create New Hotel' : 'Edit Hotel'}
            </h3>
            <form onSubmit={onSubmit} className="space-y-5">
              <input
                type="text"
                name="name"
                placeholder="Hotel Name"
                value={form.name}
                onChange={onChangeForm}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <input
                type="text"
                name="country"
                placeholder="Country"
                value={form.country}
                onChange={onChangeForm}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <input
                type="text"
                name="city"
                placeholder="City"
                value={form.city}
                onChange={onChangeForm}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <input
                type="text"
                name="address"
                placeholder="Address"
                value={form.address}
                onChange={onChangeForm}
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <div className="flex justify-end gap-4">
                <button
                  type="button"
                  onClick={() => {
                    setEditingHotel(null)
                    resetForm()
                  }}
                  className="btn-secondary px-4 py-2"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={createOrUpdateLoading}
                  className="btn-primary px-4 py-2"
                >
                  {createOrUpdateLoading ? 'Saving...' : 'Save'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default HotelManagement