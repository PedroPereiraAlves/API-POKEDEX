import React, { useEffect, useState } from 'react';
import PokemonCards from '../components/PokemonCards';
import NavBar from '../components/NavBar';
import axios from "axios";
import { Container, Grid } from '@mui/material';

export const Home = () => {

  const [pokemons, setPokemons] = useState([]);

  useEffect(() => {
    GetAllPokemons()
  }, []);

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

  const pokemonFiltrado = async (nomepokemon) => {
    try {
      if (!nomepokemon) {
        GetAllPokemons();
      }
      const response = await axios.get(`http://localhost:5195/api/v1/pokemon/${nomepokemon}`);
      if (response.data) {
        setPokemons(response.data); 
      } else {
        console.error(`Erro na busca do Pokémon com nome ${nomepokemon}`);
      }
    } catch (error) {
      console.error('Erro na requisição:', error);
    }
  }

  return (
    <div>
      <NavBar pokemonFiltrado={pokemonFiltrado}/>
      <Container maxWidth="xg">
        <Grid container spacing={3}>
          {pokemons.map((pokemon, key) => (
              <Grid item xs={2} key={key}>
                <PokemonCards nomepokemon={pokemon.nomepokemon}
                              url={pokemon.url}
                />  
              </Grid>
          ))}
        </Grid>
      </Container>
    </div>
  )
}
