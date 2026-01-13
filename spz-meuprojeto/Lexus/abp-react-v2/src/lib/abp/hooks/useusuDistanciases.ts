import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuDistanciasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuDistanciases(input: GetUsuDistanciasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuDistanciases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-distancias", {
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

export function useAllUsuDistanciases() {
  return useQuery({
    queryKey: ["usuDistanciases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-distancias", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuDistancias(id: string) {
  return useQuery({
    queryKey: ["usuDistancias", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-distancias/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuDistancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-distancias", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuDistanciases"] });
    },
  });
}

export function useUpdateUsuDistancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-distancias/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuDistanciases"] });
      queryClient.invalidateQueries({ queryKey: ["usuDistancias", data.id] });
    },
  });
}

export function useDeleteUsuDistancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-distancias/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuDistanciases"] });
    },
  });
}


