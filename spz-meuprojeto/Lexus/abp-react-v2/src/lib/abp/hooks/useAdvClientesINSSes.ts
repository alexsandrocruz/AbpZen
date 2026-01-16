import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesINSSesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesINSSes(input: GetAdvClientesINSSesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesINSSes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-inss", {
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

export function useAllAdvClientesINSSes() {
  return useQuery({
    queryKey: ["advClientesINSSes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-inss", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesINSS(id: string) {
  return useQuery({
    queryKey: ["advClientesINSS", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-inss/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-inss", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSes"] });
    },
  });
}

export function useUpdateAdvClientesINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-inss/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSes"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesINSS", data.id] });
    },
  });
}

export function useDeleteAdvClientesINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-inss/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSes"] });
    },
  });
}


