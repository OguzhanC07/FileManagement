import React, { useState, useContext } from "react";
import "semantic-ui-css/semantic.min.css";
import { Button, Form, Grid, Segment } from "semantic-ui-react";

import "./Login.css";
import { AuthContext, SIGNIN } from "../../context/AuthContext";
import * as authService from "../../services/authService";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const { dispatch } = useContext(AuthContext);

  const loginHandler = async (e) => {
    e.preventDefault();
    setError(null);
    if (username !== "" && password !== "") {
      setIsLoading(true);
      try {
        const response = await authService.signin(username, password);
        setIsLoading(false);

        dispatch({
          type: SIGNIN,
          user: {
            token: response.data.token,
            userId: response.data.id,
            expirationDate: response.data.expirationDate,
            isAuth: true,
          },
        });
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    } else {
      setError("Kullanıcı adı ve şifre gereklidir.");
    }
  };

  return (
    <Grid className="container" centered columns={2}>
      <Grid.Column>
        <Segment inverted>
          <h2 style={{ textAlign: "center", fontFamily: "cursive" }}>
            File Management
          </h2>
          <Form inverted loading={isLoading}>
            <Form.Field>
              <Form.Input
                fluid
                label="Username"
                placeholder="Username"
                id="username"
                type="text"
                onChange={(e) => setUsername(e.target.value)}
              />
            </Form.Field>
            <Form.Field>
              <Form.Input
                fluid
                id="password"
                label="Password"
                placeholder="Password"
                type="password"
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Field>
            <Button onClick={(e) => loginHandler(e)} primary>
              Login
            </Button>
            {error && (
              <p style={{ textAlign: "center", color: "red" }}>{error}</p>
            )}
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default Login;
