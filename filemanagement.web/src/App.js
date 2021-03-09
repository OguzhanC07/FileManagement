import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import AuthContextProvider from "./context/AuthContext";
import Login from "./Pages/Login/Login";

function App() {
  return (
    <AuthContextProvider>
      <Router>
        <Switch>
          <Route path="/" component={Login} />
        </Switch>
      </Router>
    </AuthContextProvider>
  );
}

export default App;
