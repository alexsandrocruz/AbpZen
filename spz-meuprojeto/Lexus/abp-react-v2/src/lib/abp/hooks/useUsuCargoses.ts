import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuCargosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuCargoses(input: GetUsuCargosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuCargoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-cargos", {
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

export function useAllUsuCargoses() {
  return useQuery({
    queryKey: ["usuCargoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-cargos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuCargos(id: string) {
  return useQuery({
    queryKey: ["usuCargos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-cargos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-cargos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuCargoses"] });
    },
  });
}

export function useUpdateUsuCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-cargos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuCargoses"] });
      queryClient.invalidateQueries({ queryKey: ["usuCargos", data.id] });
    },
  });
}

export function useDeleteUsuCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-cargos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuCargoses"] });
    },
  });
}


