import React from 'react'

const DeleteConfirmationModal = ({ onConfirm, onCancel, children }) => (
  <div
    className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
    onClick={onCancel}
  >
    <div
      className="bg-white rounded-lg shadow-xl max-w-md w-full p-6"
      onClick={(e) => e.stopPropagation()}
    >
      <h3 className="text-xl font-semibold mb-5 text-gray-900">Confirm Deletion</h3>
      <div className="mb-6 text-gray-700">{children}</div>
      <div className="flex justify-end space-x-4">
        <button
          onClick={onCancel}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 transition"
          type="button"
        >
          Cancel
        </button>
        <button
          onClick={onConfirm}
          className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700 transition"
          type="button"
        >
          Delete
        </button>
      </div>
    </div>
  </div>
)

export default DeleteConfirmationModal