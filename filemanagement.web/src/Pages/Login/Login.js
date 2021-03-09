import React, { useState } from "react";
import "semantic-ui-css/semantic.min.css";
import { Button, Form, Grid, Segment } from "semantic-ui-react";

import "./Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const submitFormHandler = () => {
    setIsLoading(true);
    setError(null);
    if (email !== "" && password !== "") {
      //login process
      setIsLoading(false);
    } else {
      setError("Kullanıcı adı ve şifre gereklidir.");
      setIsLoading(false);
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
                type="text"
                onChange={(e) => setEmail(e.target.value)}
              />
            </Form.Field>
            <Form.Field>
              <Form.Input
                fluid
                label="Password"
                placeholder="Password"
                type="password"
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Field>
            <Button onClick={submitFormHandler} primary>
              Login
            </Button>
            <p style={{ textAlign: "center", color: "red" }}>{error}</p>
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default Login;
