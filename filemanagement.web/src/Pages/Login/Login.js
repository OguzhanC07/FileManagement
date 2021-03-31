import React, { useState, useContext } from "react";
import "semantic-ui-css/semantic.min.css";
import { Button, Form, Grid, Segment } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import "./Login.css";
import { AuthContext, SIGNIN } from "../../context/AuthContext";
import * as authService from "../../services/authService";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { t } = useTranslation();
  const { dispatch } = useContext(AuthContext);

  const loginHandler = async (e) => {
    e.preventDefault();
    setError(null);
    if (username.trim().trimStart() !== "" && password !== "") {
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
        if (error.message === "connection")
          setError(t("responseErrors.connection"));
        else if (error.message === "wentWrong")
          setError(t("responseErrors.wentWrong"));
        else setError(error.message);
        setIsLoading(false);
      }
    } else {
      setError(t("loginScreen.loginError"));
    }
  };

  return (
    <Grid className="container" centered columns={2}>
      <Grid.Column>
        <Segment inverted>
          <h2 className="title">File Management</h2>
          <Form inverted loading={isLoading}>
            <Form.Field>
              <Form.Input
                fluid
                label={t("loginScreen.username")}
                placeholder={t("loginScreen.username")}
                id="username"
                type="text"
                onChange={(e) => setUsername(e.target.value)}
              />
            </Form.Field>
            <Form.Field>
              <Form.Input
                fluid
                id="password"
                label={t("loginScreen.password")}
                placeholder={t("loginScreen.password")}
                type="password"
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Field>
            <Button onClick={(e) => loginHandler(e)} primary>
              {t("loginScreen.login")}
            </Button>
            {error && <p className="errorMessage">{error}</p>}
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default Login;
