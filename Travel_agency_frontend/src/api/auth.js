import axios from 'axios'

const api = axios.create({
  baseURL: '/api/Auth'
})

export default {
  login(email, password) {
    return api.post('/login', { email, password })
  },
  register(username, email, password) {
    return api.post('/register', { username, email, password })
  }
}