import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabFormasRecebimentosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabFormasRecebimentos(input: GetFabFormasRecebimentosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabFormasRecebimentos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-formas-recebimento", {
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

export function useAllFabFormasRecebimentos() {
  return useQuery({
    queryKey: ["fabFormasRecebimentos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-formas-recebimento", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabFormasRecebimento(id: string) {
  return useQuery({
    queryKey: ["fabFormasRecebimento", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-formas-recebimento/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabFormasRecebimento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-formas-recebimento", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasRecebimentos"] });
    },
  });
}

export function useUpdateFabFormasRecebimento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-formas-recebimento/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasRecebimentos"] });
      queryClient.invalidateQueries({ queryKey: ["fabFormasRecebimento", data.id] });
    },
  });
}

export function useDeleteFabFormasRecebimento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-formas-recebimento/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasRecebimentos"] });
    },
  });
}


