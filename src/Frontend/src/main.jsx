import { createRoot } from 'react-dom/client'
import { createBrowserRouter, Navigate, RouterProvider } from 'react-router'
import HomePage from './pages/HomePage.jsx';
import Page from './layout/Page.jsx';
import AttractionPage from './pages/AttractionPage.jsx';
import SearchPage from './pages/SearchPage.jsx';
import LoginPage from './pages/LoginPage.jsx';
import RegisterPage from './pages/RegisterPage.jsx';
import ProfilePage from './pages/ProfilePage.jsx';
import App from './App.jsx';
import CreateAttraction from './pages/CreateAttraction.jsx';
import AdminPage from './pages/AdminPage.jsx';
import EditAttraction from './pages/EditAttraction.jsx';

const router = createBrowserRouter([
  {
    path: "",
    Component: Page,
    children: [
      {
        path: "/",
        element: <Navigate to="/login" />
      },
      {
        path: "/attraction/:id",
        Component: AttractionPage,
      },
      {
        path: "/search",
        Component: SearchPage,
      },
      {
        path: "/login",
        Component: LoginPage,
      },
      {
        path: "/register",
        Component: RegisterPage,
      },
      {
        path: "/profile",
        Component: ProfilePage,
      },
      {
        path: "/create",
        Component: CreateAttraction,
      },
      {
        path: "/edit/:id",
        Component: EditAttraction,
      },
      {
        path: "/admin",
        Component: AdminPage,
      },

    ]
  }
]);

createRoot(document.getElementById('root')).render(
  <App>
    <RouterProvider router={router} />
  </App>
)
