import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesModelosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesModeloses(input: GetAdvClientesModelosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesModeloses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-modelos", {
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

export function useAllAdvClientesModeloses() {
  return useQuery({
    queryKey: ["advClientesModeloses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-modelos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesModelos(id: string) {
  return useQuery({
    queryKey: ["advClientesModelos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-modelos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesModelos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-modelos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesModeloses"] });
    },
  });
}

export function useUpdateAdvClientesModelos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-modelos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesModeloses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesModelos", data.id] });
    },
  });
}

export function useDeleteAdvClientesModelos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-modelos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesModeloses"] });
    },
  });
}


