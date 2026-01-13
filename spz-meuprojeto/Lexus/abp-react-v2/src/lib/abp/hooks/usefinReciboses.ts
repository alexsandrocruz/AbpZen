import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinRecibosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinReciboses(input: GetFinRecibosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finReciboses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-recibos", {
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

export function useAllFinReciboses() {
  return useQuery({
    queryKey: ["finReciboses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-recibos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinRecibos(id: string) {
  return useQuery({
    queryKey: ["finRecibos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-recibos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinRecibos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-recibos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finReciboses"] });
    },
  });
}

export function useUpdateFinRecibos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-recibos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finReciboses"] });
      queryClient.invalidateQueries({ queryKey: ["finRecibos", data.id] });
    },
  });
}

export function useDeleteFinRecibos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-recibos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finReciboses"] });
    },
  });
}


