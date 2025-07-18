import React from 'react';
import {BrowserRouter as Router, Link, Route, Routes} from 'react-router-dom';
import {AppBar, Button, Container, Toolbar, Typography} from '@mui/material';
import VehicleMakesPage from './pages/VehicleMakesPage';
import VehicleModelsPage from './pages/VehicleModelsPage';
import VehicleOwnersPage from './pages/VehicleOwnersPage';
import VehicleRegistrationsPage from './pages/VehicleRegistrationsPage';
import VehicleEngineTypesPage from "./pages/EngineTypesPage.tsx";

const App: React.FC = () => {
    return (
        <Router>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" component="div" sx={{flexGrow: 1}}>
                        Vehicle Management System
                    </Typography>
                    <Button color="inherit" component={Link} to="/makes">Makes</Button>
                    <Button color="inherit" component={Link} to="/models">Models</Button>
                    <Button color="inherit" component={Link} to="/owners">Owners</Button>
                    <Button color="inherit" component={Link} to="/engines">Engine types</Button>
                    <Button color="inherit" component={Link} to="/registrations">Registrations</Button>
                </Toolbar>
            </AppBar>
            <Container maxWidth="lg" sx={{mt: 4, mb: 4}}>
                <Routes>
                    <Route path="/makes" element={<VehicleMakesPage/>}/>
                    <Route path="/models" element={<VehicleModelsPage/>}/>
                    <Route path="/owners" element={<VehicleOwnersPage/>}/>
                    <Route path="/engines" element={<VehicleEngineTypesPage/>}/>
                    <Route path="/registrations" element={<VehicleRegistrationsPage/>}/>
                    <Route path="/" element={<VehicleMakesPage/>}/>
                </Routes>
            </Container>
        </Router>
    );
};

export default App;