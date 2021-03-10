import AuthContextProvider from "./context/AuthContext";
import CollectionContainer from "./routes/CollectionContainer";

function App() {
  return (
    <AuthContextProvider>
      <CollectionContainer />
    </AuthContextProvider>
  );
}
export default App;
