import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesHistoricosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesHistoricoses(input: GetAdvClientesHistoricosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesHistoricoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-historicos", {
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

export function useAllAdvClientesHistoricoses() {
  return useQuery({
    queryKey: ["advClientesHistoricoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-historicos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesHistoricos(id: string) {
  return useQuery({
    queryKey: ["advClientesHistoricos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-historicos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-historicos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesHistoricoses"] });
    },
  });
}

export function useUpdateAdvClientesHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-historicos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesHistoricoses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesHistoricos", data.id] });
    },
  });
}

export function useDeleteAdvClientesHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-historicos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesHistoricoses"] });
    },
  });
}


