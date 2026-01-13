import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFlwConfigExcecoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFlwConfigExcecoeses(input: GetFlwConfigExcecoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["flwConfigExcecoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-config-excecoes", {
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

export function useAllFlwConfigExcecoeses() {
  return useQuery({
    queryKey: ["flwConfigExcecoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-config-excecoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFlwConfigExcecoes(id: string) {
  return useQuery({
    queryKey: ["flwConfigExcecoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/flw-config-excecoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFlwConfigExcecoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/flw-config-excecoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigExcecoeses"] });
    },
  });
}

export function useUpdateFlwConfigExcecoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/flw-config-excecoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigExcecoeses"] });
      queryClient.invalidateQueries({ queryKey: ["flwConfigExcecoes", data.id] });
    },
  });
}

export function useDeleteFlwConfigExcecoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/flw-config-excecoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigExcecoeses"] });
    },
  });
}


