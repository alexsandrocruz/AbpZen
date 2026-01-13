import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetArtistSpecialtiesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  artistId?: string;
  }

export function useArtistSpecialties(input: GetArtistSpecialtiesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["artistSpecialties", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/artist-specialty", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllArtistSpecialties() {
  return useQuery({
    queryKey: ["artistSpecialties", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/artist-specialty", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useArtistSpecialty(id: string) {
  return useQuery({
    queryKey: ["artistSpecialty", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/artist-specialty/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateArtistSpecialty() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/artist-specialty", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["artistSpecialties"] });
    },
  });
}

export function useUpdateArtistSpecialty() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/artist-specialty/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["artistSpecialties"] });
      queryClient.invalidateQueries({ queryKey: ["artistSpecialty", data.id] });
    },
  });
}

export function useDeleteArtistSpecialty() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/artist-specialty/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["artistSpecialties"] });
    },
  });
}


