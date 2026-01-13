import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesConvertidosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesConvertidoses(input: GetAdvClientesConvertidosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesConvertidoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-convertidos", {
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

export function useAllAdvClientesConvertidoses() {
  return useQuery({
    queryKey: ["advClientesConvertidoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-convertidos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesConvertidos(id: string) {
  return useQuery({
    queryKey: ["advClientesConvertidos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-convertidos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesConvertidos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-convertidos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesConvertidoses"] });
    },
  });
}

export function useUpdateAdvClientesConvertidos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-convertidos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesConvertidoses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesConvertidos", data.id] });
    },
  });
}

export function useDeleteAdvClientesConvertidos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-convertidos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesConvertidoses"] });
    },
  });
}


