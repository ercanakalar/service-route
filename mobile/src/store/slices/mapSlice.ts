import { createSlice } from '@reduxjs/toolkit';

const initialState: any = {
  wayPoints: [],
  isLoading: false,
  state: 'initial',
  user: null,
  error: null,
  errors: null,
};

export const mapSlice = createSlice({
  name: 'map',
  initialState,
  reducers: {},
  extraReducers: (builder) => {},
});

export default mapSlice.reducer;
