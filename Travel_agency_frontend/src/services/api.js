import axios from 'axios'

export const apiClient = axios.create({
    baseURL: 'http://localhost:8080',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  function getUserIdFromToken() {
    const token = localStorage.getItem('token')
    if (!token) return null
  
    try {
      const payload = JSON.parse(atob(token.split('.')[1]))
      return payload.sub || payload.id || null
    } catch {
      return null
    }
  }

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status;
    const data = error.response?.data;

    if (status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }

    if (status === 403) {
      window.location.href = '/forbidden';
    }

    if (status === 404) {
      window.location.href = '/not-found';
    }

    if (status === 409) {
      window.location.href = '/conflict';
    }

    if (status === 400 && data?.error === 'Validation Error') {
      window.location.href = '/validation-error';
    }

    if (status === 500) {
      window.location.href = '/server-error';
    }

    return Promise.reject(error);
  }
);

export const authAPI = {
  login: (email, password) => apiClient.post('/api/Auth/login', { email, password }),
  register: (username, email, password) => apiClient.post('/api/Auth/register', { username, email, password }),
}

export const hotelAPI = {
  getAll: () => apiClient.get('/api/Hotel'),
  getById: (id) => apiClient.get(`/api/Hotel/${id}`),
  create: (data) => apiClient.post('/api/Hotel', data),
  update: (id, data) => apiClient.put(`/api/Hotel/${id}`, data),
  delete: (id) => apiClient.delete(`/api/Hotel/${id}`),
}

export const hotelRoomAPI = {
  getAll: () => apiClient.get('/api/HotelRoom'),
  getById: (id) => apiClient.get(`/api/HotelRoom/${id}`),
  getByHotelId: (hotelId) => apiClient.get(`/api/HotelRoom/hotel/${hotelId}/rooms`),
  create: (data) => apiClient.post('/api/HotelRoom', data),
  update: (id, data) => apiClient.put(`/api/HotelRoom/${id}`, data),
  delete: (id) => apiClient.delete(`/api/HotelRoom/${id}`),
}

export const hotelBookingAPI = {
  getAll: () => apiClient.get('/api/HotelBooking'),
  getById: (id) => apiClient.get(`/api/HotelBooking/${id}`),
  create: (data) => apiClient.post('/api/HotelBooking', data),
  update: (id, data) => apiClient.put(`/api/HotelBooking/${id}`, data),
  delete: (id) => apiClient.delete(`/api/HotelBooking/${id}`),
}

export const tourAPI = {
  getAll: () => apiClient.get('/api/Tour'),
  getPaged: (pageNumber = 1, pageSize = 10) => 
    apiClient.get(`/api/Tour/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`),
  getById: (id) => apiClient.get(`/api/Tour/${id}`),
  create: (data) => apiClient.post('/api/Tour', data),
  update: (id, data) => apiClient.put(`/api/Tour/${id}`, data),
  delete: (id) => apiClient.delete(`/api/Tour/${id}`),
  filter: (filterData) => apiClient.post('/api/TourFilter/filter', filterData),
  search: (searchQuery) => apiClient.get(`/api/TourFilter/search/${searchQuery}`),
}

export const tourBookingAPI = {
  getAll: () => apiClient.get('/api/TourBooking'),
  getById: (id) => apiClient.get(`/api/TourBooking/${id}`),
  create: (data) => apiClient.post('/api/TourBooking', data),
  update: (id, data) => apiClient.put(`/api/TourBooking/${id}`, data),
  delete: (id) => apiClient.delete(`/api/TourBooking/${id}`),
}

export const transportAPI = {
  getAll: () => apiClient.get('/api/Transport'),
  getById: (id) => apiClient.get(`/api/Transport/${id}`),
  create: (data) => apiClient.post('/api/Transport', data),
  update: (id, data) => apiClient.put(`/api/Transport/${id}`, data),
  delete: (id) => apiClient.delete(`/api/Transport/${id}`),
}

export const ticketBookingAPI = {
  getAll: () => apiClient.get('/api/TicketBooking'),
  getById: (id) => apiClient.get(`/api/TicketBooking/${id}`),
  create: (data) => apiClient.post('/api/TicketBooking', data),
  update: (id, data) => apiClient.put(`/api/TicketBooking/${id}`, data),
  delete: (id) => apiClient.delete(`/api/TicketBooking/${id}`),
}

export const userAPI = {
    getAll: () => apiClient.get('/api/User'),
    getById: (id) => apiClient.get(`/api/User/${id}`),
    getCurrentUser: () => {
        const id = getUserIdFromToken()
        if (!id) return Promise.reject(new Error('User id not found'))
        return apiClient.get(`/api/User/${id}`)
      },
    getByEmail: (email) => apiClient.get(`/api/User/email/${email}`),
    update: (id, data) => apiClient.put(`/api/User/${id}`, data),
    delete: (id) => apiClient.delete(`/api/User/${id}`),
    updateRole: (id, role) => apiClient.put(`/api/User/role/${id}`, role ),
  }