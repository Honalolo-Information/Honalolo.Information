import { useState } from "react";
import AuthContext from "./contexts/AuthContext"

function App(props) {
  const [auth, setAuth] = useState(false);

  const authValue = {
    value: auth,
    setValue: setAuth,
  };

  return (
      <AuthContext.Provider value={authValue} >
        {props.children}
      </AuthContext.Provider>
  )
}

export default App
