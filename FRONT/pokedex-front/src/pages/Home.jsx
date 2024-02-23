import React, { useEffect, useState } from 'react';
import PokemonCards from '../components/PokemonCards';
import NavBar from '../components/NavBar';
import axios from "axios";
import { Container, Grid } from '@mui/material';

export const Home = () => {

  const [pokemons, setPokemons] = useState([]);
  const [fotopokemons, setFotoPokemons] = useState([]);

  useEffect(() => {
    GetAllPokemons()
    GetFotoPokemonCompony()
  }, []);

  const GetFotoPokemonCompony = () => {
      var endpoints = [];
      for(var i = 0; i++; i < 50){
        endpoints.push(`https://pokeapi.co/api/v2/pokemon/${i}`);
      }
      
       axios
      .all(endpoints.map((endpoint) => axios.get(endpoint)))
      .then((responses) => {
        const fotoPokemons = responses.map((response) => ({
          nomepokemon: response.data.name,
          foto: response.data.sprites.front_default,
        }));
        setFotoPokemons(fotoPokemons);
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  }
  const GetAllPokemons = () => {
  axios
  .get("http://localhost:5195/api/v1/pokemon/")
  .then((response) => {
    console.log("API response:", response.data);  // Adicionando console.log
    setPokemons(response.data);
  })
  .catch(error => {
    console.error("Error fetching data:", error);
  });
}
  return (
    <div>
      <NavBar/>
      <Container maxWidth="xg">
        <Grid container>
          {pokemons.map((pokemon, key) => (
              <Grid item xs={3} key={key}>
                <PokemonCards nomepokemon={pokemon.nomepokemon}/>
              </Grid>
          ))}
        </Grid>
      </Container>
    </div>
  )
}
