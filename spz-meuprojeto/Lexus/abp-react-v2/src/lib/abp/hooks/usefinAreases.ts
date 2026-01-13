import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinAreasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinAreases(input: GetFinAreasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finAreases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-areas", {
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

export function useAllFinAreases() {
  return useQuery({
    queryKey: ["finAreases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-areas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinAreas(id: string) {
  return useQuery({
    queryKey: ["finAreas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-areas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-areas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finAreases"] });
    },
  });
}

export function useUpdateFinAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-areas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finAreases"] });
      queryClient.invalidateQueries({ queryKey: ["finAreas", data.id] });
    },
  });
}

export function useDeleteFinAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-areas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finAreases"] });
    },
  });
}


