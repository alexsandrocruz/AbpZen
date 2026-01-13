import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinCentrosResultadosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinCentrosResultados(input: GetFinCentrosResultadosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finCentrosResultados", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-centros-resultado", {
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

export function useAllFinCentrosResultados() {
  return useQuery({
    queryKey: ["finCentrosResultados", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-centros-resultado", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinCentrosResultado(id: string) {
  return useQuery({
    queryKey: ["finCentrosResultado", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-centros-resultado/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinCentrosResultado() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-centros-resultado", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosResultados"] });
    },
  });
}

export function useUpdateFinCentrosResultado() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-centros-resultado/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosResultados"] });
      queryClient.invalidateQueries({ queryKey: ["finCentrosResultado", data.id] });
    },
  });
}

export function useDeleteFinCentrosResultado() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-centros-resultado/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosResultados"] });
    },
  });
}


