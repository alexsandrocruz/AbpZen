import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFdtDevsesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFdtDevses(input: GetFdtDevsesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fdtDevses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fdt-devs", {
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

export function useAllFdtDevses() {
  return useQuery({
    queryKey: ["fdtDevses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fdt-devs", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFdtDevs(id: string) {
  return useQuery({
    queryKey: ["fdtDevs", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fdt-devs/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFdtDevs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fdt-devs", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fdtDevses"] });
    },
  });
}

export function useUpdateFdtDevs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fdt-devs/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fdtDevses"] });
      queryClient.invalidateQueries({ queryKey: ["fdtDevs", data.id] });
    },
  });
}

export function useDeleteFdtDevs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fdt-devs/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fdtDevses"] });
    },
  });
}


